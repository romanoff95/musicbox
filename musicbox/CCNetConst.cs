using System;
using System.Collections.Generic;
using System.Text;

namespace musicbox
{
    public partial class CCCRSProtocol
    {
        const byte SYNC = 0x02;	//!< synchronization byte 
        const byte ACK = 0x00;	//!< ACK code
        const byte NAK = 0xFF;	//!< NAK code
        const byte ST_INV_CMD = 0x30;//!< INVALID COMMAND response

        /**	\defgroup Addr Device Addresses
        * @{
        */
        const byte ADDR_BB = 0x01; //!< Address for Bill-To-Bill units
        const byte ADDR_CHANGER = 0x02; //!< Address for Coin Changer
        const byte ADDR_FL = 0x03; //!< Address for Bill Validators
        const byte ADDR_CR = 0x04; //!< Address for Smart Card Reader
        /**@} */

        /**	\defgroup Cmds Interface commands
        * @{
        */
        const byte RESET = 0x30; //!<REST command code
        const byte GET_STATUS = 0x31; //!<STATUS REQUEST command code
        const byte SET_SECURITY = 0x32; //!<SET SECURITY command code
        const byte POLL = 0x33; //!<POLL command code
        const byte BILL_TYPE = 0x34; //!<BILL TYPE command code
        const byte PACK = 0x35; //!<PACK command code
        const byte RETURN = 0x36; //!<RETURN command code
        const byte IDENTIFICATION = 0x37;//!<IDENTIFICATION command code
        const byte IDENT_EXT = 0x3E;//!<EXTENDED IDENTIFICATION command code
        const byte HOLD = 0x38;//!<HOLD command code
        const byte C_STATUS = 0x3B;//!<RECYCLING CASSETTE STATUS REQUEST command code
        const byte DISPENSE = 0x3C;//!<DISPENSE command code
        const byte UNLOAD = 0x3D;//!<UNLOAD command code
        const byte SET_CASSETES = 0x40;//!<SET RECYCLING CASSETTE TYPE command code
        const byte GET_BILL_TABLE = 0x41;//!<BILL TABLE REQUEST command code	
        const byte DOWNLOAD = 0x50;//!<DOWNLOAD command code
        const byte CRC32 = 0x51;//!<CRC32 REQUEST command code
        const byte SET_TIME = 0x62;//!<SET BB TIME command code
        const byte SET_BAR_PARAMS = 0x39;//!<SET BARCODE PARAMETERS command code
        const byte EXTRACT_BAR_DATA = 0x3A;//!<EXTRACT BARCODE DATA command code
        const byte POWER_RECOVERY = 0x66;//!<POWER RECOVERY command code
        const byte EMPTY_DISPENSER = 0x67;//!<EMPTY DISPENSER command code
        const byte SET_OPTIONS = 0x68;//!<SET OPTIONS command code
        const byte GET_OPTIONS = 0x69;//!<GET OPTIONS command code
        /**@} */

        /**	\defgroup Options Options
        * Describes options supported by CCNET (as a bitmap)
        * @{
        */
        //Options (bitmap)
        const UInt32 OPT_LED_INHIBIT = 0x80000000;//!< Turn OFF LEDs of the bezel in the DISABLED state
        const UInt32 OPT_KEEP_BILL = 0x40000000;//!< Hold bill after ejection on the input roller
        const UInt32 OPT_LOOK_TAPE = 0x20000000; //!< Use improved algorithm for tape detection
        const UInt32 OPT_TURN_SWITCH = 0x10000000; //!< Turn switch after packing a bill
        /**@} */
        // States
        /**	\defgroup States CCNET states and events
        * 
        * @{
        */
        const byte ST_POWER_UP = 0x10;//!< POWER UP state
        const byte ST_POWER_BILL_ESCROW = 0x11;//!< POWER UP WITH BILL IN ESCROW state
        const byte ST_POWER_BILL_STACKER = 0x12;//!< POWER UP WITH BILL IN STACKER state
        const byte ST_INITIALIZE = 0x13;//!< INITIALIZING state
        const byte ST_IDLING = 0x14;//!< IDLING state
        const byte ST_ACCEPTING = 0x15;//!< ACCEPTING state
        public const byte ST_PACKING = 0x17;//!< STACKING/PACKING state
        const byte ST_RETURNING = 0x18;//!< RETURNING state
        public const byte ST_DISABLED = 0x19;//!< UNIT DISABLED state
        const byte ST_HOLDING = 0x1A;//!< HOLDING state
        const byte ST_BUSY = 0x1B;//!< Device is busy
        const byte ST_REJECTING = 0x1C;//!< REJECTING state. Followed by a rejection code
        //Rejection codes
        /**	\defgroup RCodes Rejection codes
        * 
        * @{
        */
        const byte RJ_INSERTION = 0x60; //!< Rejection because of insertion problem
        const byte RJ_MAGNETIC = 0x61; //!< Rejection because of invalid magnetic pattern
        const byte RJ_REMAINING = 0x62; //!< Rejection because of other bill remaining in the device
        const byte RJ_MULTIPLYING = 0x63; //!< Rejection because of multiple check failures
        const byte RJ_CONVEYING = 0x64; //!< Rejection because of conveying 
        const byte RJ_IDENT = 0x65; //!< Rejection because of identification failure
        const byte RJ_VRFY = 0x66; //!< Rejection because of verification failure
        const byte RJ_OPT = 0x67; //!< Rejection because of optical pattern mismatch
        const byte RJ_INHIBIT = 0x68; //!< Rejection because the denomination is inhibited
        const byte RJ_CAP = 0x69; //!< Rejection because of capacity sensor pattern mismatch
        const byte RJ_OPERATION = 0x6A; //!< Rejection because of operation error
        const byte RJ_LNG = 0x6C; //!< Rejection because of invalid bill length
        const byte RJ_UV = 0x6D; //!< Rejection because of invalid UV pattern
        const byte RJ_BAR = 0x92; //!< Rejection because of unrecognized barcode
        const byte RJ_BAR_LNG = 0x93; //!< Rejection because of invalid barcode length
        const byte RJ_BAR_START = 0x94; //!< Rejection because of invalid barcode start sequence
        const byte RJ_BAR_STOP = 0x95; //!< Rejection because of invalid barcode stop sequence
        /**@} */

        const byte ST_DISPENSING = 0x1D;//!< DISPENSING state
        const byte ST_UNLOADING = 0x1E;//!< UNLOADING state 
        const byte ST_SETTING_CS_TYPE = 0x21;//!< SETTING RECYCLING CASSETTE TYPE state
        const byte ST_DISPENSED = 0x25;//!< DISPENSED event
        const byte ST_UNLOADED = 0x26;//!< UNLOADED event
        const byte ST_BILL_NUMBER_ERR = 0x28;//!< INVALID BILL NUMBER event
        const byte ST_CS_TYPE_SET = 0x29;//!< RECYCLING CASSETTE TYPE SET event
        const byte ST_ST_FULL = 0x41;//!< DROP CASSETTE IS FULL state
        public const byte ST_BOX = 0x42;//!< DROP CASSETTE REMOVED state 
        const byte ST_BV_JAMMED = 0x43;//!< JAM IN VALIDATOR state
        const byte ST_ST_JAMMED = 0x44;//!< JAM IN STACKER state
        const byte ST_CHEATED = 0x45;//!< CHEATED event
        const byte ST_PAUSED = 0x46;//!< PAUSED state
        const byte ST_FAILURE = 0x47;//!< FAILURE state

        //Failure codes
        /**	\defgroup FCodes Failure codes
        * 
        * @{
        */
        const byte FLR_STACKER = 0x50; //!< Stacking mechanism failure
        const byte FLR_TR_SPEED = 0x51; //!< Invalid speed of transport mechanism
        const byte FLR_TRANSPORT = 0x52; //!< Transport mechanism failure
        const byte FLR_ALIGNING = 0x53; //!< Aligning mechanism failure
        const byte FLR_INIT_CAS = 0x54; //!< Initial cassette status failure
        const byte FLR_OPT = 0x65; //!< Optical channel failure
        const byte FLR_MAG = 0x66; //!< Inductive channel failure
        const byte FLR_CAP = 0x67; //!< Capacity sensor failure
        /**@} */

        // Credit events
        public const byte ST_PACKED = 0x81;	/**< A bill has been packed. 2nd byte - 0xXY:
															\n X-bill type
															\n Y-Packed into:
															\n 0-BOX, else - Cassette Y;

											*/
        const byte ESCROW = 0x80; //!< A bill is held in the escrow position	
        const byte RETURNED = 0x82; //!< A bill was returned
        /**@} */
        // Cassetes status
        /**	\defgroup CSStatus Possible cassette status codes
        * 
        * @{
        */
        const byte CS_OK = 0;	//!< Cassette is present and operational
        const byte CS_FULL = 1; //!< Cassette is full
        const byte CS_NU = 0xFE;//!< Cassette is not present
        const byte CS_MALFUNCTION = 0xFF;//!< Cassette is malfunctioning
        const byte CS_NA = 0xFD;//!< Cassette is not assigned to any denomination
        const byte CS_ESCROW = 0xFC;//!< Cassette is assigned to multi-escrow 
        /**@} */
        /**	\defgroup BTs Predefined bill type values
        * 
        * @{
        */
        const byte BT_ESCROW = 24; //!< Bill type associated with the escrow cassette
        const byte BT_NO_TYPE = 0x1f; //!< Invalid bill type
        const byte BT_BAR = 23; //!< Bill type associated with barcode coupon
        /**@} */
        // Error codes 
        /**	\defgroup ErrCode CCNET Interface error codes
        *
        * @{
        */
        /** \defgroup CErrs Communication error codes
	
              The codes related to phisical data transmission and frame integrity

           @{
        */
        const int RE_NONE = 0; //!< No error happened
        const int RE_TIMEOUT = -1;//!< Communication timeout
        const int RE_SYNC = -2;//!< Synchronization error (invalid synchro byte)
        const int RE_DATA = -3;//!< Data reception error
        /**@} */
        const int RE_CRC = -4;//!< CRC error

        /** \defgroup LErrs Logical error codes

          The codes related to the interface logic

           @{
        */
        const int ER_NAK = -5;//!< NAK received
        const int ER_INVALID_CMD = -6;//!< Invalid command response received 
        const int ER_EXECUTION = -7;//!< Execution error response received
        const int ERR_INVALID_STATE = -8;//!< Invalid state received
    }
    class CCommand
    {
        private Int32 m_Code = 0;

        public Int32 Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        private byte[] m_Data = new byte[4096];

        public byte[] Data
        {
            get { return m_Data; }
        }
        public CCommand(byte[] inData, int Code, int iLen)
        {
            if ((m_Code = Code) == 0)
            {
                if (iLen == 0)
                    for (int i = 0; i < inData[2]; i++) Data[i] = inData[i];
                else
                    for (int i = 0; i < iLen; i++) Data[i] = inData[i];
            }
        }
        public CCommand()
        {
            m_Code = 0;
        }
        public void SetByte(byte Byte, int Index)
        {
            m_Data[Index] = Byte;
        }
        string ToStr(String pStr)
        {
            //char sTmp[16];
            //strcpy(pStr,"");
            pStr = "";
            String sTmp = "";
            int iLen = (m_Data[2] != 0) ? m_Data[2] : ((m_Data[4] << 8) + Data[5]);
            for (int i = 0; i < iLen; i++)
            {
                pStr += String.Format(sTmp, "{0X:00} ", m_Data[i]);
                //strcat(pStr, sTmp);
            }
            return pStr;
        }
    }
}
