using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace musicbox
{
    public partial class CCCRSProtocol
    {
        private struct _Identification
        {
            // Identification command fields
            public String PartNumber;//!< Firmware part number 
            public String SN;//!< Device's serial number
            public UInt64 DS1;//!< Device's asset number
            // Extended identification command fiels
            public String BVBootVersion;//!< Boot version of the validating head (is reported in response to EXTENDED IDENTIFICATION command)
            public String BVVersion;//!< Firmware version of the validating head (is reported in response to EXTENDED IDENTIFICATION command)

            public String BCCPUBoot;//!< Boot version of the central controller (is reported in response to EXTENDED IDENTIFICATION command)
            public String BCCPUVersion;//!<Firmware version of the central controller (is reported in response to EXTENDED IDENTIFICATION command)

            public String BCDispenserBoot;//!< Boot version of the dispenser (is reported in response to EXTENDED IDENTIFICATION command)
            public String BCDispenserVersion;//!< Firmware version of the dispenser (is reported in response to EXTENDED IDENTIFICATION command)

            public String BCCS1Boot;//!< Boot version of the cassette 1 (is reported in response to EXTENDED IDENTIFICATION command)
            public String BCCS2Boot;//!< Boot version of the cassette 2 (is reported in response to EXTENDED IDENTIFICATION command)
            public String BCCS3Boot;//!< Boot version of the cassette 3 (is reported in response to EXTENDED IDENTIFICATION command)
            public String BCCSVersion;//!< Firmware version of the cassettes (is reported in response to EXTENDED IDENTIFICATION command)
        }
        public struct _PollResults
        {
            public byte Z1,//!< State
                 Z2;//!< State extension or substate

        }  //!< A variable keeping last POLL result
        private struct _BillStatus
        {
            public UInt32 Enabled; //!< A bitmap describing which bill types are enabled
            public UInt32 Security; //!< A bitmap describing which bill types are processed in High Security mode
            public UInt32 Routing; //!< A bitmap describing which denominations are routed to a recycling cassette. Is a valid value only for BB units
        }//!< Variable containing the most recent response to the STATUS REQUEST
        public struct _BillRecord
        {
            public float Denomination;		//!< Denomination of the bill
            public String sCountryCode;
            public bool bRouted;			//!< A bool variable specifiying whether the bill is forwarded to a cassette
        }

        private _Identification Ident = new _Identification();
        private _PollResults m_PollResults = new _PollResults();
        private List<_BillRecord> m_BillRecordList = new List<_BillRecord>();

        public List<_BillRecord> BillRecordList
        {
            get { return m_BillRecordList; }
            //set { m_BillRecordList = value; }
        }


        public _PollResults PollResults
        {
            get { return m_PollResults; }
            //set { m_PollResults = value; }
        }
        private _BillStatus BillStatus = new _BillStatus();

        private int iCmdDelay;	//!< Delay between two consequtive commands
        private int iBytesToRecieve = 0;
        private int iRecievingError = 0;
        private CCommand cmdIn; //!< A variable to store current device responses
        private CCommand cmdOut;//!< A variable to store controller commands
        private int iLastError; //!< A variable storing error code generated during last serial I/O 

        private IntPtr m_SerialPort;

        

        public CCCRSProtocol(IntPtr serialPort)
        {
            m_SerialPort = serialPort;

            iCmdDelay = 20;
            Ident.BCCPUBoot = "N/A";
            Ident.BCCPUVersion = "N/A";
            Ident.BCCS1Boot = "N/A";
            Ident.BCCS2Boot = "N/A";
            Ident.BCCS3Boot = "N/A";
            Ident.BCCSVersion = "N/A";
            Ident.BCDispenserBoot = "N/A";
            Ident.BCDispenserVersion = "N/A";
            Ident.BVBootVersion = "N/A";
            Ident.BVVersion = "N/A";
            Ident.PartNumber = "N/A";
        }

        int SendCommand(Byte[] BufOut, ref Byte[] BufIn)
        {
            iRecievingError = RE_TIMEOUT;
            for (int iErrCount = 0; iErrCount < 1; iErrCount++)
            {
                iBytesToRecieve = 6;
                Win32Com.PurgeComm(m_SerialPort, Win32Com.PURGE_RXABORT | Win32Com.PURGE_TXABORT | Win32Com.PURGE_TXCLEAR | Win32Com.PURGE_RXCLEAR);
                if (BufOut[2] == 0)
                    Win32Com.Send(m_SerialPort, BufOut, ((UInt16)BufOut[4] << 8) + BufOut[5]);
                else
                    Win32Com.Send(m_SerialPort, BufOut, BufOut[2]);
                if ((BufOut[3] == ACK) || (BufOut[3] == NAK))
                    return iRecievingError = RE_NONE;

                if (Win32Com.Recieve(m_SerialPort, out BufIn, iBytesToRecieve))
                {
                    if (BufIn[0] != SYNC)
                        iRecievingError = RE_SYNC;
                    else
                    {
                        int iLen = ((BufIn[2] != 0) ? BufIn[2] : (BufIn[5] + ((UInt16)BufIn[4] << 8))) - iBytesToRecieve;
                        if (iLen > 0)
                        {
                            byte[] tmpBuf;
                            byte[] tmpBufEx;
                            //byte[] tmpBuf = new byte[BufIn.Length - iBytesToRecieve];
                            //Array.Copy(BufIn, iBytesToRecieve, tmpBuf, 0, tmpBuf.Length);
                            if (Win32Com.Recieve(m_SerialPort, out tmpBuf, iLen))
                            {
                                tmpBufEx = new byte[iBytesToRecieve + tmpBuf.Length];
                                Array.Copy(BufIn, 0, tmpBufEx, 0, iBytesToRecieve);
                                Array.Copy(tmpBuf, 0, tmpBufEx, iBytesToRecieve, tmpBuf.Length);
                                BufIn = tmpBufEx;
                                iRecievingError = RE_NONE;
                                break;
                            }
                            else
                            {
                                tmpBufEx = new byte[iBytesToRecieve + tmpBuf.Length];
                                Array.Copy(BufIn, 0, tmpBufEx, 0, iBytesToRecieve);
                                Array.Copy(tmpBuf, 0, tmpBufEx, iBytesToRecieve, tmpBuf.Length);
                                BufIn = tmpBufEx;
                                iRecievingError = RE_DATA;
                                Win32Com.PurgeComm(m_SerialPort, Win32Com.PURGE_RXABORT | Win32Com.PURGE_RXCLEAR);
                            }
                        }
                        else
                        {
                            iRecievingError = RE_NONE;
                            break;
                        }
                    }
                }
            }
            return iRecievingError;
        }
        UInt16 crc16_ccitt(Byte data, UInt16 crc)
        {
            UInt16 a = 0x8408, d = crc, i;
            d ^= data;
            for (i = 0; i < 8; i++)
            {
                if ((d & 0x0001) != 0)
                {
                    d >>= 1;
                    d ^= a;
                }
                else d >>= 1;
            }
            return d;
        }
        UInt16 CalculateCRC(Byte[] pBuffer)
        {
            UInt16 wCRC = 0, Len = (pBuffer[2] != 0) ?
                (Byte)pBuffer[2] : (UInt16)((UInt16)((pBuffer)[4] << 8) + (pBuffer)[5]);
            for (int i = 0; i < Len - 2; i++)
                wCRC = crc16_ccitt(pBuffer[i], wCRC);
            return wCRC;
        }

        CCommand TransmitCMD(CCommand Cmd, byte Addr)
        {
            byte[] tmpBuffer = new byte[256];
            int i = (Cmd.Data[2] != 0) ? (Cmd.Data)[2] :
                                            (((UInt16)(Cmd.Data)[4]) << 8) + (Cmd.Data)[5];
            Cmd.SetByte(Addr, 1);
            UInt16 wCRC = CalculateCRC(Cmd.Data);
            Cmd.SetByte((byte)wCRC, i - 2);
            Cmd.SetByte((byte)(wCRC >> 8), i - 1);
            cmdOut = Cmd;

            int iErrCode = SendCommand(Cmd.Data, ref tmpBuffer);
            if ((iErrCode == 0) && (Cmd.Data[3] != 0) && (0xFF != Cmd.Data[3]))
            {
                wCRC = (UInt16)(tmpBuffer[((tmpBuffer[2] != 0) ? tmpBuffer[2] - 2 : (UInt16)((tmpBuffer)[4] << 8) + tmpBuffer[5] - 2)] +
                    (tmpBuffer[((tmpBuffer[2] != 0) ? tmpBuffer[2] - 1 : (UInt16)(tmpBuffer[4] << 8) + tmpBuffer[5] - 1)] << 8));
                if (CalculateCRC(tmpBuffer) != wCRC)
                    iErrCode = RE_CRC;
                cmdIn = new CCommand(tmpBuffer, iErrCode, (tmpBuffer[2] != 0) ? (tmpBuffer)[2] :
                                            (((UInt16)(tmpBuffer)[4]) << 8) + (tmpBuffer)[5]);
                return cmdIn;
            }
            cmdIn = new CCommand(tmpBuffer, iErrCode, 0);
            return cmdIn;
        }
        CCommand Transmit(CCommand CMD, byte Addr)
        {
            CCommand cmdRes = null;
            CCommand cmdACK = new CCommand();
            for (int i = 0; i < 3; i++)
            {
                cmdRes = TransmitCMD(CMD, Addr);
                cmdACK.SetByte(SYNC, 0);
                cmdACK.SetByte(6, 2);
                cmdACK.SetByte(ACK, 3);

                if (cmdRes.Code == RE_NONE)
                {

                    if ((ACK == cmdRes.Data[3]) && (cmdRes.Data[2] == 6))
                    {
                        return cmdRes;
                    }
                    if ((NAK == cmdRes.Data[3]) && (cmdRes.Data[2] == 6))
                    {

                        if (iCmdDelay != 0)
                            Thread.Sleep(iCmdDelay);//5	
                    }
                    else
                    {
                        cmdACK.SetByte(ACK, 3);
                        TransmitCMD(cmdACK, Addr);
                        if (iCmdDelay != 0)
                            Thread.Sleep(iCmdDelay);//5
                        break;
                    }
                }
                else
                {
                    if (cmdRes.Code != RE_TIMEOUT)
                    {
                        cmdACK.SetByte(NAK, 3);
                        TransmitCMD(cmdACK, Addr);
                        if (iCmdDelay != 0)
                            Thread.Sleep(iCmdDelay);//5
                    }
                }
            }

            return cmdRes;
        }

        public bool CmdReset(byte Addr)
        {
            byte[] Data = new byte[] { SYNC, 0, 6, RESET, 0 };
            CCommand cmd = new CCommand(Data, 0, 6 - 1);
            CCommand Response = Transmit(cmd, Addr);
            byte ack;
            if ((iLastError = Response.Code) == 0)
            {

                if ((ack = Response.Data[3]) != ACK)
                {
                    iLastError = (ack != ST_INV_CMD) ? ER_NAK : ER_INVALID_CMD;
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return false;
            }
        }
        public bool CmdPoll(byte Addr)
        {
            byte[] Data = new byte[] { SYNC, 0, 6, POLL, 0, 0 };
            CCommand cmd = new CCommand(Data, 0, 0);
            CCommand Response = Transmit(cmd, Addr);
            if ((iLastError = Response.Code) == 0)
            {
                m_PollResults.Z1 = Response.Data[3];
                m_PollResults.Z2 = Response.Data[4];
                return true;
            }
            else
            {
                m_PollResults.Z1 = 0;
                m_PollResults.Z2 = 0;
                return false;
            }
        }
        public bool CmdStatus(byte Addr)
        {
            byte[] Data = new byte[] { SYNC, 0, 6, GET_STATUS, 0, 0 };
            CCommand cmd = new CCommand(Data, 0, 0);
            CCommand Response = Transmit(cmd, Addr);
            if ((iLastError = Response.Code) == 0)
            {
                if ((Response.Data[3] == ST_INV_CMD) && (Response.Data[2] == 6))
                {
                    iLastError = ER_INVALID_CMD;
                    BillStatus.Enabled = 0;
                    BillStatus.Security = 0;
                    BillStatus.Routing = 0;
                    return false;
                }
                BillStatus.Enabled = Response.Data[5] + ((UInt32)Response.Data[4] << 8) + ((UInt32)Response.Data[3] << 16);
                BillStatus.Security = Response.Data[8] + ((UInt32)Response.Data[7] << 8) + ((UInt32)Response.Data[6] << 16);
                BillStatus.Routing = Response.Data[11] + ((UInt32)Response.Data[10] << 8) + ((UInt32)Response.Data[9] << 16);
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool CmdGetBillTable( byte Addr)
        {
            byte[] Data = new byte[] { SYNC, 0, 6, GET_BILL_TABLE, 0, 0 };
            CCommand cmd = new CCommand(Data, 0, 0);
            CCommand Response = Transmit(cmd, Addr);
            if ((iLastError = Response.Code) == 0)
            {
                if ((Response.Data[3] == ST_INV_CMD) && (Response.Data[2] == 6))
                {
                    iLastError = ER_INVALID_CMD;
                    /*for (int i = 0; i < 24; i++)
                    {
                        m_BillTable[i].Denomination = 0;
                        //strcpy(BillTable[i].sCountryCode,"");
                    }*/
                    /*foreach (_BillRecord record in m_BillRecordList)
                    {
                        record.Denomination = 0;
                        record.sCountryCode1 = "";
                        record.sCountryCode2 = "";
                        record.sCountryCode3 = "";
                    }*/
                    return false;
                }
                m_BillRecordList.Clear();
                for(int i=0;i<(Response.Data[2])-5;i+=5)
                {
                    _BillRecord billRecord = new _BillRecord();
                    billRecord.Denomination = Response.Data[i + 3];
                    billRecord.sCountryCode = String.Format("{0}{1}{2}",
                    Response.Data[i + 4],Response.Data[i + 5],Response.Data[i + 6]);
                    //BillTable[i/5].Denomination=Response.Data[i+3];
                    //char sTmp[5];
                    //strncpy(sTmp,(const char *)(Response.GetData()+i+4),3);
                    //sTmp[3]='\0';
                    //strcpy(BillTable[i/5].sCountryCode,sTmp);
                    if(((Response.Data[i+7])&0x80) != 0)
                    {
                        for (int j = 0; j < ((Response.Data[i + 7]) & 0x7F); j++)
                            billRecord.Denomination /= 10;
                            //BillTable[i/5].Denomination/=10;
                    }
                    else
                    {
                        for (int j = 0; j < ((Response.Data[i + 7]) & 0x7F); j++)
                            billRecord.Denomination *= 10;
                            //BillTable[i/5].Denomination*=10;
                    };
                    m_BillRecordList.Add(billRecord);
                }
                /*for(i;i<24*5;i+=5)
                {
                    BillTable[i/5].Denomination=0;
                    strcpy(BillTable[i/5].sCountryCode,"");
                } */
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool CmdBillType(UInt32 enBill, UInt32 escBill, byte Addr)
        {
            byte[] Data = new byte[] {SYNC,0,12,BILL_TYPE,
					(byte)(enBill>>16),(byte)(enBill>>8),(byte)enBill,
					(byte)(escBill>>16),(byte)(escBill>>8),(byte)escBill,
					0,0};
            CCommand cmd = new CCommand(Data, 0, 0);
            CCommand Response = Transmit(cmd, Addr);
            byte ack;
            if ((iLastError = Response.Code) == 0)
            {
                if ((ack = Response.Data[3]) != ACK)
                {
                    iLastError = (ack != ST_INV_CMD) ? ER_NAK : ER_INVALID_CMD;
                    return false;
                }
                else return true;

            }
            else
            {
                return false;
            }
        }
    }
}
