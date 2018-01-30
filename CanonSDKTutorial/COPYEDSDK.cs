using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace CanonSDKTutorial
{
    /// <summary>
    /// 对EDSDK.DLL程序集的方法包裹
    /// </summary>
    [SecurityCritical]
    public class COPYEDSDK
    {
        /// <summary>
        /// 定义edsdk.dll文件的路径，Path to the EDSDK DLL
        /// </summary>
        const string DLLPath = @"EDSDK\EDSDK.dll";

        #region 定义数据类型，Data Types
        public enum EdsDataType : uint
        {
            Unknown = 0,
            Bool = 1,
            String = 2,
            Int8 = 3,
            UInt8 = 6,
            Int16 = 4,
            UInt16 = 7,
            Int32 = 8,
            UInt32 = 9,
            Int64 = 10,
            UInt64 = 11,
            Float = 12,
            Double = 13,
            ByteBlock = 14,
            Rational = 20,
            Point = 21,
            Rect = 22,
            Time = 23,
            Bool_Array = 30,
            Int8_Array = 31,
            Int16_Array = 32,
            Int32_Array = 33,
            UInt8_Array = 34,
            UInt16_Array = 35,
            UInt32_Array = 36,
            Rational_Array = 37,
            FocusInfo = 101,
            PictureStyleDesc,
        } 
        #endregion

        #region 定义相机状态命令，Camera status command
        /*----------------------------------
         Camera Status Commands
        ----------------------------------*/
        public const uint CameraState_UILock = 0x00000000;
        public const uint CameraState_UIUnLock = 0x00000001;
        public const uint CameraState_EnterDirectTransfer = 0x00000002;
        public const uint CameraState_ExitDirectTransfer = 0x00000003;   
        #endregion

        #region 定义相机命令，包含两个结构体，Camera commands
        /*-----------------------------------------------------------------------------
         Send Commands
        -----------------------------------------------------------------------------*/
        public const uint CameraCommand_TakePicture = 0x00000000;
        public const uint CameraCommand_ExtendShutDownTimer = 0x00000001;
        public const uint CameraCommand_BulbStart = 0x00000002;
        public const uint CameraCommand_BulbEnd = 0x00000003;
        public const uint CameraCommand_DoEvfAf = 0x00000102;
        public const uint CameraCommand_DriveLensEvf = 0x00000103;
        public const uint CameraCommand_DoClickWBEvf = 0x00000104;
        public const uint CameraCommand_PressShutterButton = 0x00000004;

        public enum EdsEvfAf : uint
        {
            CameraCommand_EvfAf_OFF = 0,
            CameraCommand_EvfAf_ON = 1,
        }

        public enum EdsShutterButton : uint
        {
            CameraCommand_ShutterButton_OFF = 0x00000000,
            CameraCommand_ShutterButton_Halfway = 0x00000001,
            CameraCommand_ShutterButton_Completely = 0x00000003,
            CameraCommand_ShutterButton_Halfway_NonAF = 0x00010001,
            CameraCommand_ShutterButton_Completely_NonAF = 0x00010003,
        } 
        #endregion

        #region 定义属性IDs，Property IDs

        /*----------------------------------
         Camera Setting Properties
        ----------------------------------*/
        public const uint PropID_Unknown = 0x0000ffff;
        public const uint PropID_ProductName = 0x00000002;
        public const uint PropID_BodyIDEx = 0x00000015;
        public const uint PropID_OwnerName = 0x00000004;
        public const uint PropID_MakerName = 0x00000005;
        public const uint PropID_DateTime = 0x00000006;
        public const uint PropID_FirmwareVersion = 0x00000007;
        public const uint PropID_BatteryLevel = 0x00000008;
        public const uint PropID_CFn = 0x00000009;
        public const uint PropID_SaveTo = 0x0000000b;
        public const uint kEdsPropID_CurrentStorage = 0x0000000c;
        public const uint kEdsPropID_CurrentFolder = 0x0000000d;
        public const uint kEdsPropID_MyMenu = 0x0000000e;
        public const uint PropID_BatteryQuality = 0x00000010;
        public const uint PropID_HDDirectoryStructure = 0x00000020;

        /*----------------------------------
         Image Properties
        ----------------------------------*/
        public const uint PropID_ImageQuality = 0x00000100;
        public const uint PropID_JpegQuality = 0x00000101;
        public const uint PropID_Orientation = 0x00000102;
        public const uint PropID_ICCProfile = 0x00000103;
        public const uint PropID_FocusInfo = 0x00000104;
        public const uint PropID_DigitalExposure = 0x00000105;
        public const uint PropID_WhiteBalance = 0x00000106;
        public const uint PropID_ColorTemperature = 0x00000107;
        public const uint PropID_WhiteBalanceShift = 0x00000108;
        public const uint PropID_Contrast = 0x00000109;
        public const uint PropID_ColorSaturation = 0x0000010a;
        public const uint PropID_ColorTone = 0x0000010b;
        public const uint PropID_Sharpness = 0x0000010c;
        public const uint PropID_ColorSpace = 0x0000010d;
        public const uint PropID_ToneCurve = 0x0000010e;
        public const uint PropID_PhotoEffect = 0x0000010f;
        public const uint PropID_FilterEffect = 0x00000110;
        public const uint PropID_ToningEffect = 0x00000111;
        public const uint PropID_ParameterSet = 0x00000112;
        public const uint PropID_ColorMatrix = 0x00000113;
        public const uint PropID_PictureStyle = 0x00000114;
        public const uint PropID_PictureStyleDesc = 0x00000115;
        public const uint PropID_PictureStyleCaption = 0x00000200;

        /*----------------------------------
         Image Processing Properties
        ----------------------------------*/
        public const uint PropID_Linear = 0x00000300;
        public const uint PropID_ClickWBPoint = 0x00000301;
        public const uint PropID_WBCoeffs = 0x00000302;

        /*----------------------------------
         Property Mask
        ----------------------------------*/
        public const uint PropID_AtCapture_Flag = 0x80000000;

        /*----------------------------------
         Capture Properties
        ----------------------------------*/
        public const uint PropID_AEMode = 0x00000400;
        public const uint PropID_AEModeSelect = 0x00000436;
        public const uint PropID_DriveMode = 0x00000401;
        public const uint PropID_ISOSpeed = 0x00000402;
        public const uint PropID_MeteringMode = 0x00000403;
        public const uint PropID_AFMode = 0x00000404;
        public const uint PropID_Av = 0x00000405;
        public const uint PropID_Tv = 0x00000406;
        public const uint PropID_ExposureCompensation = 0x00000407;
        public const uint PropID_FlashCompensation = 0x00000408;
        public const uint PropID_FocalLength = 0x00000409;
        public const uint PropID_AvailableShots = 0x0000040a;
        public const uint PropID_Bracket = 0x0000040b;
        public const uint PropID_WhiteBalanceBracket = 0x0000040c;
        public const uint PropID_LensName = 0x0000040d;
        public const uint PropID_AEBracket = 0x0000040e;
        public const uint PropID_FEBracket = 0x0000040f;
        public const uint PropID_ISOBracket = 0x00000410;
        public const uint PropID_NoiseReduction = 0x00000411;
        public const uint PropID_FlashOn = 0x00000412;
        public const uint PropID_RedEye = 0x00000413;
        public const uint PropID_FlashMode = 0x00000414;
        public const uint PropID_LensStatus = 0x00000416;
        public const uint PropID_Artist = 0x00000418;
        public const uint PropID_Copyright = 0x00000419;
        public const uint PropID_DepthOfField = 0x0000041b;
        public const uint PropID_EFCompensation = 0x0000041e;

        /*----------------------------------
         EVF Properties
        ----------------------------------*/
        public const uint PropID_Evf_OutputDevice = 0x00000500;
        public const uint PropID_Evf_Mode = 0x00000501;
        public const uint PropID_Evf_WhiteBalance = 0x00000502;
        public const uint PropID_Evf_ColorTemperature = 0x00000503;
        public const uint PropID_Evf_DepthOfFieldPreview = 0x00000504;
        public const uint PropID_Evf_Zoom = 0x00000507;
        public const uint PropID_Evf_ZoomPosition = 0x00000508;
        public const uint PropID_Evf_FocusAid = 0x00000509;
        public const uint PropID_Evf_Histogram = 0x0000050A;
        public const uint PropID_Evf_ImagePosition = 0x0000050B;
        public const uint PropID_Evf_HistogramStatus = 0x0000050C;
        public const uint PropID_Evf_AFMode = 0x0000050E;
        public const uint PropID_Record = 0x00000510;

        /*----------------------------------
         Image GPS Properties
        ----------------------------------*/
        public const uint PropID_GPSVersionID = 0x00000800;
        public const uint PropID_GPSLatitudeRef = 0x00000801;
        public const uint PropID_GPSLatitude = 0x00000802;
        public const uint PropID_GPSLongitudeRef = 0x00000803;
        public const uint PropID_GPSLongitude = 0x00000804;
        public const uint PropID_GPSAltitudeRef = 0x00000805;
        public const uint PropID_GPSAltitude = 0x00000806;
        public const uint PropID_GPSTimeStamp = 0x00000807;
        public const uint PropID_GPSSatellites = 0x00000808;
        public const uint PropID_GPSStatus = 0x00000809;
        public const uint PropID_GPSMapDatum = 0x00000812;
        public const uint PropID_GPSDateStamp = 0x0000081D; 		

        #endregion

        #region 定义事件IDs,Event IDs

        /*-----------------------------------------------------------------------------
         Camera Events
        -----------------------------------------------------------------------------*/
        /* No camera events. */

        /*----------------------------------
         Property Event
        ----------------------------------*/
        /* Notifies all property events. */
        public const uint PropertyEvent_All = 0x00000100;

        /* Notifies that a camera property value has been changed. 
         The changed property can be retrieved from event data. 
         The changed value can be retrieved by means of EdsGetPropertyData. 
         In the case of type 1 protocol standard cameras, 
         notification of changed properties can only be issued for custom functions (CFn). 
         If the property type is 0x0000FFFF, the changed property cannot be identified. 
         Thus, retrieve all required properties repeatedly. */
        public const uint PropertyEvent_PropertyChanged = 0x00000101;

        /* Notifies of changes in the list of camera properties with configurable values. 
         The list of configurable values for property IDs indicated in event data 
          can be retrieved by means of EdsGetPropertyDesc. 
         For type 1 protocol standard cameras, the property ID is identified as "Unknown"
          during notification. 
          Thus, you must retrieve a list of configurable values for all properties and
          retrieve the property values repeatedly. 
         (For details on properties for which you can retrieve a list of configurable
          properties, 
          see the description of EdsGetPropertyDesc). */
        public const uint PropertyEvent_PropertyDescChanged = 0x00000102;

        /*----------------------------------
         Object Event
        ----------------------------------*/
        /* Notifies all object events. */
        public const uint ObjectEvent_All = 0x00000200;

        /* Notifies that the volume object (memory card) state (VolumeInfo)
          has been changed. 
         Changed objects are indicated by event data. 
         The changed value can be retrieved by means of EdsGetVolumeInfo. 
         Notification of this event is not issued for type 1 protocol standard cameras. */
        public const uint ObjectEvent_VolumeInfoChanged = 0x00000201;

        /* Notifies if the designated volume on a camera has been formatted.
         If notification of this event is received, get sub-items of the designated
          volume again as needed. 
         Changed volume objects can be retrieved from event data. 
         Objects cannot be identified on cameras earlier than the D30
          if files are added or deleted. 
         Thus, these events are subject to notification. */
        public const uint ObjectEvent_VolumeUpdateItems = 0x00000202;

        /* Notifies if many images are deleted in a designated folder on a camera.
         If notification of this event is received, get sub-items of the designated
          folder again as needed. 
         Changed folders (specifically, directory item objects) can be retrieved
          from event data. */
        public const uint ObjectEvent_FolderUpdateItems = 0x00000203;

        /* Notifies of the creation of objects such as new folders or files
          on a camera compact flash card or the like. 
         This event is generated if the camera has been set to store captured
          images simultaneously on the camera and a computer,
          for example, but not if the camera is set to store images
          on the computer alone. 
         Newly created objects are indicated by event data. 
         Because objects are not indicated for type 1 protocol standard cameras,
          (that is, objects are indicated as NULL),
         you must again retrieve child objects under the camera object to 
         identify the new objects. */
        public const uint ObjectEvent_DirItemCreated = 0x00000204;

        /* Notifies of the deletion of objects such as folders or files on a camera
          compact flash card or the like. 
         Deleted objects are indicated in event data. 
         Because objects are not indicated for type 1 protocol standard cameras, 
         you must again retrieve child objects under the camera object to
          identify deleted objects. */
        public const uint ObjectEvent_DirItemRemoved = 0x00000205;

        /* Notifies that information of DirItem objects has been changed. 
         Changed objects are indicated by event data. 
         The changed value can be retrieved by means of EdsGetDirectoryItemInfo. 
         Notification of this event is not issued for type 1 protocol standard cameras. */
        public const uint ObjectEvent_DirItemInfoChanged = 0x00000206;

        /* Notifies that header information has been updated, as for rotation information
          of image files on the camera. 
         If this event is received, get the file header information again, as needed. 
         This function is for type 2 protocol standard cameras only. */
        public const uint ObjectEvent_DirItemContentChanged = 0x00000207;

        /* Notifies that there are objects on a camera to be transferred to a computer. 
         This event is generated after remote release from a computer or local release
          from a camera. 
         If this event is received, objects indicated in the event data must be downloaded.
          Furthermore, if the application does not require the objects, instead
          of downloading them,
           execute EdsDownloadCancel and release resources held by the camera. 
         The order of downloading from type 1 protocol standard cameras must be the order
          in which the events are received. */
        public const uint ObjectEvent_DirItemRequestTransfer = 0x00000208;

        /* Notifies if the camera's direct transfer button is pressed. 
         If this event is received, objects indicated in the event data must be downloaded. 
         Furthermore, if the application does not require the objects, instead of
          downloading them, 
          execute EdsDownloadCancel and release resources held by the camera. 
         Notification of this event is not issued for type 1 protocol standard cameras. */
        public const uint ObjectEvent_DirItemRequestTransferDT = 0x00000209;

        /* Notifies of requests from a camera to cancel object transfer 
          if the button to cancel direct transfer is pressed on the camera. 
         If the parameter is 0, it means that cancellation of transfer is requested for
          objects still not downloaded,
          with these objects indicated by kEdsObjectEvent_DirItemRequestTransferDT. 
         Notification of this event is not issued for type 1 protocol standard cameras. */
        public const uint ObjectEvent_DirItemCancelTransferDT = 0x0000020a;

        /* Volume Added. */
        public const uint ObjectEvent_VolumeAdded = 0x0000020c;

        /* Volume Removed. */
        public const uint ObjectEvent_VolumeRemoved = 0x0000020d;

        /*----------------------------------
         State Event
        ----------------------------------*/
        /* Notifies all state events. */
        public const uint StateEvent_All = 0x00000300;

        /* Indicates that a camera is no longer connected to a computer, 
         whether it was disconnected by unplugging a cord, opening
          the compact flash compartment, 
          turning the camera off, auto shut-off, or by other means. */
        public const uint StateEvent_Shutdown = 0x00000301;

        /* Notifies of whether or not there are objects waiting to
          be transferred to a host computer. 
         This is useful when ensuring all shot images have been transferred 
         when the application is closed. 
         Notification of this event is not issued for type 1 protocol 
         standard cameras. */
        public const uint StateEvent_JobStatusChanged = 0x00000302;

        /* Notifies that the camera will shut down after a specific period. 
         Generated only if auto shut-off is set. 
         Exactly when notification is issued (that is, the number of
          seconds until shutdown) varies depending on the camera model. 
         To continue operation without having the camera shut down,
         use EdsSendCommand to extend the auto shut-off timer.
         The time in seconds until the camera shuts down is returned
          as the initial value. */
        public const uint StateEvent_WillSoonShutDown = 0x00000303;

        /* As the counterpart event to kEdsStateEvent_WillSoonShutDown,
         this event notifies of updates to the number of seconds until
          a camera shuts down. 
         After the update, the period until shutdown is model-dependent. */
        public const uint StateEvent_ShutDownTimerUpdate = 0x00000304;

        /* Notifies that a requested release has failed, due to focus
          failure or similar factors. */
        public const uint StateEvent_CaptureError = 0x00000305;

        /* Notifies of internal SDK errors. 
         If this error event is received, the issuing device will probably
          not be able to continue working properly,
          so cancel the remote connection. */
        public const uint StateEvent_InternalError = 0x00000306;

        /* Af Result. */
        public const uint StateEvent_AfResult = 0x00000309;

        /* Bulb Exposure Time. */
        public const uint StateEvent_BulbExposureTime = 0x00000310;	

        #endregion

        #region 定义基础结构，Definition of base Structures
        /// <summary>
        /// 命名最大字节数
        /// </summary>
        public const int EDS_MAX_NAME = 256;

        /// <summary>
        /// 块的尺寸
        /// </summary>
        public const int EDS_TRANSFER_BLOCK_SIZE = 512;

        /*-----------------------------------------------------------------------------
         Point
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsPoint
        {
            public int x;
            public int y;
        }

        /*-----------------------------------------------------------------------------
         Rectangle
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsRect
        {
            public int x;
            public int y;
            public int width;
            public int height;
        }

        /*-----------------------------------------------------------------------------
         Size
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsSize
        {
            public int width;
            public int height;
        }

        /*-----------------------------------------------------------------------------
         Rational
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsRational
        {
            public int Numerator;
            public uint Denominator;
        }

        /*-----------------------------------------------------------------------------
         Time
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsTime
        {
            public int Year;
            public int Month;
            public int Day;
            public int Hour;
            public int Minute;
            public int Second;
            public int Milliseconds;
        }

        /*-----------------------------------------------------------------------------
         Device Info
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsDeviceInfo
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = EDS_MAX_NAME)]
            public string szPortName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = EDS_MAX_NAME)]
            public string szDeviceDescription;

            public uint DeviceSubType;

            public uint reserved;
        }

        /*-----------------------------------------------------------------------------
         Volume Info
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsVolumeInfo
        {
            public uint StorageType;
            public uint Access;
            public ulong MaxCapacity;
            public ulong FreeSpaceInBytes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = EDS_MAX_NAME)]
            public string szVolumeLabel;
        }


        /*-----------------------------------------------------------------------------
         DirectoryItem Info
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsDirectoryItemInfo
        {
            public uint Size;
            public int isFolder;
            public uint GroupID;
            public uint Option;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = EDS_MAX_NAME)]
            public string szFileName;

            public uint format;
            public uint dateTime;
        }


        /*-----------------------------------------------------------------------------
         Image Info
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsImageInfo
        {
            public uint Width;
            public uint Height;

            public uint NumOfComponents;
            public uint ComponentDepth;

            public EdsRect EffectiveRect;

            public uint reserved1;
            public uint reserved2;

        }

        /*-----------------------------------------------------------------------------
         SaveImage Setting
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsSaveImageSetting
        {
            public uint JPEGQuality;
            IntPtr iccProfileStream;
            public uint reserved;
        }

        /*-----------------------------------------------------------------------------
         Property Desc
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsPropertyDesc
        {
            public int Form;
            public uint Access;
            public int NumElements;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public int[] PropDesc;
        }

        /*-----------------------------------------------------------------------------
         Picture Style Desc
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsPictureStyleDesc
        {
            public int contrast;
            public uint sharpness;
            public int saturation;
            public int colorTone;
            public uint filterEffect;
            public uint toningEffect;
        }

        /*-----------------------------------------------------------------------------
         Focus Point
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsFocusPoint
        {
            public uint valid;
            public uint selected;
            public uint justFocus;
            public EdsRect rect;
            public uint reserved;
        }

        /*-----------------------------------------------------------------------------
         Focus Info
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential)]
        public struct EdsFocusInfo
        {
            public EdsRect imageRect;
            public uint pointNumber;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public EdsFocusPoint[] focusPoint;
            public uint executeMode;
        }

        /*-----------------------------------------------------------------------------
         Capacity
        -----------------------------------------------------------------------------*/
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct EdsCapacity
        {
            public int NumberOfFreeClusters;
            public int BytesPerSector;
            public int Reset;
        }

        #endregion

        #region  定义错误代码，Definition of error Codes

        /*-----------------------------------------------------------------------
           ED-SDK Error Code Masks
        ------------------------------------------------------------------------*/
        public const uint EDS_ISSPECIFIC_MASK = 0x80000000;
        public const uint EDS_COMPONENTID_MASK = 0x7F000000;
        public const uint EDS_RESERVED_MASK = 0x00FF0000;
        public const uint EDS_ERRORID_MASK = 0x0000FFFF;

        /*-----------------------------------------------------------------------
           ED-SDK Base Component IDs
        ------------------------------------------------------------------------*/
        public const uint EDS_CMP_ID_CLIENT_COMPONENTID = 0x01000000;
        public const uint EDS_CMP_ID_LLSDK_COMPONENTID = 0x02000000;
        public const uint EDS_CMP_ID_HLSDK_COMPONENTID = 0x03000000;

        /*-----------------------------------------------------------------------
           ED-SDK Functin Success Code
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_OK = 0x00000000;

        /*-----------------------------------------------------------------------
           Miscellaneous errors，二类
        ------------------------------------------------------------------------*/
        /// <summary>
        /// 通用错误标识未实现1
        /// </summary>
        public const uint EDS_ERR_UNIMPLEMENTED = 0x00000001;
        /// <summary>
        /// 内部误差
        /// </summary>                  
        public const uint EDS_ERR_INTERNAL_ERROR = 0x00000002;
        /// <summary>
        /// 内存分配错误
        /// </summary>
        public const uint EDS_ERR_MEM_ALLOC_FAILED = 0x00000003;
        /// <summary>
        /// 内存释放错误
        /// </summary>
        public const uint EDS_ERR_MEM_FREE_FAILED = 0x00000004;
        /// <summary>
        /// 操作取消
        /// </summary>
        public const uint EDS_ERR_OPERATION_CANCELLED = 0x00000005;
        /// <summary>
        /// 版本误差
        /// </summary>
        public const uint EDS_ERR_INCOMPATIBLE_VERSION = 0x00000006;
        /// <summary>
        /// 不支持
        /// </summary>
        public const uint EDS_ERR_NOT_SUPPORTED = 0x00000007;
        /// <summary>
        /// 意外的例外
        /// </summary>
        public const uint EDS_ERR_UNEXPECTED_EXCEPTION = 0x00000008;
        /// <summary>
        /// 保护违规
        /// </summary>
        public const uint EDS_ERR_PROTECTION_VIOLATION = 0x00000009;
        /// <summary>
        /// 丢失的部件
        /// </summary>
        public const uint EDS_ERR_MISSING_SUBCOMPONENT = 0x0000000A;
        /// <summary>
        /// 选择不可用
        /// </summary>
        public const uint EDS_ERR_SELECTION_UNAVAILABLE = 0x0000000B;

        /*-----------------------------------------------------------------------
           File errors ，二类，文件错误
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_FILE_IO_ERROR = 0x00000020;
        public const uint EDS_ERR_FILE_TOO_MANY_OPEN = 0x00000021;
        public const uint EDS_ERR_FILE_NOT_FOUND = 0x00000022;
        public const uint EDS_ERR_FILE_OPEN_ERROR = 0x00000023;
        public const uint EDS_ERR_FILE_CLOSE_ERROR = 0x00000024;
        public const uint EDS_ERR_FILE_SEEK_ERROR = 0x00000025;
        public const uint EDS_ERR_FILE_TELL_ERROR = 0x00000026;
        public const uint EDS_ERR_FILE_READ_ERROR = 0x00000027;
        public const uint EDS_ERR_FILE_WRITE_ERROR = 0x00000028;
        public const uint EDS_ERR_FILE_PERMISSION_ERROR = 0x00000029;
        public const uint EDS_ERR_FILE_DISK_FULL_ERROR = 0x0000002A;
        public const uint EDS_ERR_FILE_ALREADY_EXISTS = 0x0000002B;
        public const uint EDS_ERR_FILE_FORMAT_UNRECOGNIZED = 0x0000002C;
        public const uint EDS_ERR_FILE_DATA_CORRUPT = 0x0000002D;
        public const uint EDS_ERR_FILE_NAMING_NA = 0x0000002E;

        /*-----------------------------------------------------------------------
           Directory errors ，二类，路径错误
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_DIR_NOT_FOUND = 0x00000040;
        public const uint EDS_ERR_DIR_IO_ERROR = 0x00000041;
        public const uint EDS_ERR_DIR_ENTRY_NOT_FOUND = 0x00000042;
        public const uint EDS_ERR_DIR_ENTRY_EXISTS = 0x00000043;
        public const uint EDS_ERR_DIR_NOT_EMPTY = 0x00000044;

        /*-----------------------------------------------------------------------
           Property errors ，二类，属性错误
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_PROPERTIES_UNAVAILABLE = 0x00000050;
        public const uint EDS_ERR_PROPERTIES_MISMATCH = 0x00000051;
        public const uint EDS_ERR_PROPERTIES_NOT_LOADED = 0x00000053;

        /*-----------------------------------------------------------------------
           Function Parameter errors ，二类，方法参数错误
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_INVALID_PARAMETER = 0x00000060;
        public const uint EDS_ERR_INVALID_HANDLE = 0x00000061;
        public const uint EDS_ERR_INVALID_POINTER = 0x00000062;
        public const uint EDS_ERR_INVALID_INDEX = 0x00000063;
        public const uint EDS_ERR_INVALID_LENGTH = 0x00000064;
        public const uint EDS_ERR_INVALID_FN_POINTER = 0x00000065;
        public const uint EDS_ERR_INVALID_SORT_FN = 0x00000066;

        /*-----------------------------------------------------------------------
           Device errors ，二类，设备错误
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_DEVICE_NOT_FOUND = 0x00000080;
        public const uint EDS_ERR_DEVICE_BUSY = 0x00000081;
        public const uint EDS_ERR_DEVICE_INVALID = 0x00000082;
        public const uint EDS_ERR_DEVICE_EMERGENCY = 0x00000083;
        public const uint EDS_ERR_DEVICE_MEMORY_FULL = 0x00000084;
        public const uint EDS_ERR_DEVICE_INTERNAL_ERROR = 0x00000085;
        public const uint EDS_ERR_DEVICE_INVALID_PARAMETER = 0x00000086;
        public const uint EDS_ERR_DEVICE_NO_DISK = 0x00000087;
        public const uint EDS_ERR_DEVICE_DISK_ERROR = 0x00000088;
        public const uint EDS_ERR_DEVICE_CF_GATE_CHANGED = 0x00000089;
        public const uint EDS_ERR_DEVICE_DIAL_CHANGED = 0x0000008A;
        public const uint EDS_ERR_DEVICE_NOT_INSTALLED = 0x0000008B;
        public const uint EDS_ERR_DEVICE_STAY_AWAKE = 0x0000008C;
        public const uint EDS_ERR_DEVICE_NOT_RELEASED = 0x0000008D;

        /*-----------------------------------------------------------------------
           Stream errors ，二类，流错误
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_STREAM_IO_ERROR = 0x000000A0;
        public const uint EDS_ERR_STREAM_NOT_OPEN = 0x000000A1;
        public const uint EDS_ERR_STREAM_ALREADY_OPEN = 0x000000A2;
        public const uint EDS_ERR_STREAM_OPEN_ERROR = 0x000000A3;
        public const uint EDS_ERR_STREAM_CLOSE_ERROR = 0x000000A4;
        public const uint EDS_ERR_STREAM_SEEK_ERROR = 0x000000A5;
        public const uint EDS_ERR_STREAM_TELL_ERROR = 0x000000A6;
        public const uint EDS_ERR_STREAM_READ_ERROR = 0x000000A7;
        public const uint EDS_ERR_STREAM_WRITE_ERROR = 0x000000A8;
        public const uint EDS_ERR_STREAM_PERMISSION_ERROR = 0x000000A9;
        public const uint EDS_ERR_STREAM_COULDNT_BEGIN_THREAD = 0x000000AA;
        public const uint EDS_ERR_STREAM_BAD_OPTIONS = 0x000000AB;
        public const uint EDS_ERR_STREAM_END_OF_STREAM = 0x000000AC;

        /*-----------------------------------------------------------------------
           Communications errors，二类，通信错误
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_COMM_PORT_IS_IN_USE = 0x000000C0;
        public const uint EDS_ERR_COMM_DISCONNECTED = 0x000000C1;
        public const uint EDS_ERR_COMM_DEVICE_INCOMPATIBLE = 0x000000C2;
        public const uint EDS_ERR_COMM_BUFFER_FULL = 0x000000C3;
        public const uint EDS_ERR_COMM_USB_BUS_ERR = 0x000000C4;

        /*-----------------------------------------------------------------------
           Lock/Unlock errors，二类，锁/解锁错误
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_USB_DEVICE_LOCK_ERROR = 0x000000D0;
        public const uint EDS_ERR_USB_DEVICE_UNLOCK_ERROR = 0x000000D1;

        /*-----------------------------------------------------------------------
           STI/WIA errors，二类
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_STI_UNKNOWN_ERROR = 0x000000E0;
        public const uint EDS_ERR_STI_INTERNAL_ERROR = 0x000000E1;
        public const uint EDS_ERR_STI_DEVICE_CREATE_ERROR = 0x000000E2;
        public const uint EDS_ERR_STI_DEVICE_RELEASE_ERROR = 0x000000E3;
        public const uint EDS_ERR_DEVICE_NOT_LAUNCHED = 0x000000E4;
        public const uint EDS_ERR_ENUM_NA = 0x000000F0;
        public const uint EDS_ERR_INVALID_FN_CALL = 0x000000F1;
        public const uint EDS_ERR_HANDLE_NOT_FOUND = 0x000000F2;
        public const uint EDS_ERR_INVALID_ID = 0x000000F3;
        public const uint EDS_ERR_WAIT_TIMEOUT_ERROR = 0x000000F4;

        /*-----------------------------------------------------------------------
           PTP errors，二类
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_SESSION_NOT_OPEN = 0x00002003;
        public const uint EDS_ERR_INVALID_TRANSACTIONID = 0x00002004;
        public const uint EDS_ERR_INCOMPLETE_TRANSFER = 0x00002007;
        public const uint EDS_ERR_INVALID_STRAGEID = 0x00002008;
        public const uint EDS_ERR_DEVICEPROP_NOT_SUPPORTED = 0x0000200A;
        public const uint EDS_ERR_INVALID_OBJECTFORMATCODE = 0x0000200B;
        public const uint EDS_ERR_SELF_TEST_FAILED = 0x00002011;
        public const uint EDS_ERR_PARTIAL_DELETION = 0x00002012;
        public const uint EDS_ERR_SPECIFICATION_BY_FORMAT_UNSUPPORTED = 0x00002014;
        public const uint EDS_ERR_NO_VALID_OBJECTINFO = 0x00002015;
        public const uint EDS_ERR_INVALID_CODE_FORMAT = 0x00002016;
        public const uint EDS_ERR_UNKNOWN_VENDER_CODE = 0x00002017;
        public const uint EDS_ERR_CAPTURE_ALREADY_TERMINATED = 0x00002018;
        public const uint EDS_ERR_INVALID_PARENTOBJECT = 0x0000201A;
        public const uint EDS_ERR_INVALID_DEVICEPROP_FORMAT = 0x0000201B;
        public const uint EDS_ERR_INVALID_DEVICEPROP_VALUE = 0x0000201C;
        public const uint EDS_ERR_SESSION_ALREADY_OPEN = 0x0000201E;
        public const uint EDS_ERR_TRANSACTION_CANCELLED = 0x0000201F;
        public const uint EDS_ERR_SPECIFICATION_OF_DESTINATION_UNSUPPORTED = 0x00002020;
        public const uint EDS_ERR_UNKNOWN_COMMAND = 0x0000A001;
        public const uint EDS_ERR_OPERATION_REFUSED = 0x0000A005;
        public const uint EDS_ERR_LENS_COVER_CLOSE = 0x0000A006;
        public const uint EDS_ERR_LOW_BATTERY = 0x0000A101;
        public const uint EDS_ERR_OBJECT_NOTREADY = 0x0000A102;

        /*-----------------------------------------------------------------------
           Capture Error，二类，拍照错误
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_TAKE_PICTURE_AF_NG = 0x00008D01;
        public const uint EDS_ERR_TAKE_PICTURE_RESERVED = 0x00008D02;
        public const uint EDS_ERR_TAKE_PICTURE_MIRROR_UP_NG = 0x00008D03;
        public const uint EDS_ERR_TAKE_PICTURE_SENSOR_CLEANING_NG = 0x00008D04;
        public const uint EDS_ERR_TAKE_PICTURE_SILENCE_NG = 0x00008D05;
        public const uint EDS_ERR_TAKE_PICTURE_NO_CARD_NG = 0x00008D06;
        public const uint EDS_ERR_TAKE_PICTURE_CARD_NG = 0x00008D07;
        public const uint EDS_ERR_TAKE_PICTURE_CARD_PROTECT_NG = 0x00008D08;

        /*-----------------------------------------------------------------------
           GENERIC Error，二类，一般性错误
        ------------------------------------------------------------------------*/
        public const uint EDS_ERR_LAST_GENERIC_ERROR_PLUS_ONE = 0x000000F5;

        #region

        #endregion



        #endregion

        #region 引用EDSDK API接口，Proto type defenition of EDSDK API



        #endregion

        #region

        #endregion
    }
}
