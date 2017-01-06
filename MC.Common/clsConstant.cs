using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MC.Common
{
    public class clsConstant
    {

        // 注册表的键值


        public const string RegEdit_Key_SoftWare = "SOFTWARE";
        public const string RegEdit_Key_AMDAPE2E = "MC";
        public const string RegEdit_Key_AMDAPE2E_ES = @"AMDAPE2E\MasterClassified";
        public const string RegEdit_Key_User = "user";
        public const string RegEdit_Key_PassWord = "password";
        public const string RegEdit_Key_Date = "date";
        public const string RegEdit_Key_Checkbox = "Checkbox";
        public const string RegEdit_Key_NPO_cloumn = "ChinaRawDataHide_cloumn";
        public const string RegEdit_Key_Save_S = "ChinaAM_Save_S ";
        public const string RegEdit_Key_assistMain = "ChinaAM_assistMain ";
        public const string RegEdit_Key_AutoNext = "ChinaAM_AutoNext ";

        //public const string RegEdit_Key_SingleRawData_cloumn = "TWSingleRawRawDataHide_cloumn";
        //public const string RegEdit_Key_Freeze_cloumn = "HKRawDataFreeze_cloumn";

        public const string RegEdit_Key_Rate = "Rate";


        // 消息对话框状态


        public const int Dialog_Status_Enable = 0;     //可以关闭
        public const int Dialog_Status_Disable = 1;     //不能关系

        // 线程进度
        public const int Thread_Progress_OK = 100;
        public const int Thread_Progress_Run = 0;

        // Excel文件的Sheet名称
        public const string Sheet_SNInfo = "SerialNum";
        public const string Sheet_ContactPerson = "Create Contact Person";
        public const string Sheet_ShipToParty = "Create Ship-to & Ordering Party";
        public const string Sheet_LinkPartnersForPartner = "Partner Link";
        public const string Sheet_CreateHLFL = "HLFL Creation";
        public const string Sheet_CreateMLFL = "MLFL Creation";
        public const string Sheet_CreateLLFL = "LLFL Creation";
        public const string Sheet_LinkMatToFL = "Add products to LLFL";
        public const string Sheet_CreateZI = "ZI Creation";
        public const string Sheet_CreateShortForm = "Short Form Mode";
        public const string Sheet_CreateZDEP = "ZDEP Mode";    //YL
        public const string Sheet_CreateRefQoute = "Create Quote With Reference";
        public const string Sheet_CreateNewQoute = "New Quote Creation";
        public const string Sheet_Discount = "Prepare Discount Form";
        public const string Sheet_RTS = "RTS";
        public const string Sheet_EOS = "EOS";

        // Doc Type
        public const string ZCRN = "ZCRN";
        public const string ZDEP = "ZDEP";
        public const string ZDEL = "ZDEL";
        public const string ZQRN = "ZQRN";
        public const string ZQ = "ZQ";
        public const string ZI = "ZI";

        // Sales Area 
        public const string HK00 = "HK00";

        // 空字符窜
        public const string EmptyString = "";

        // PO Number
        public const string INRD = "INFORMAL CONTRACT";

        // Offer
        public const string HA151AC = "HA151AC";
        public const string HA156AC = "HA156AC";
        public const string HA158AC = "HA158AC";

        // 位置0
        public const string POSN_ZERO = "000000";
        public const string POSN_ONE = "000010";

        // ES 网站
        public const string WEB_ES_URL_Main = @"https://es-int-dh.austin.hp.com/webclient/WebClient/Main.do";
        public const string WEB_ES_URL_Search = @"https://es-int-dh.austin.hp.com/webclient/WebClient/ESSearch.do";
        public const string WEB_ES_URL_Result = @"https://es-int-dh.austin.hp.com/webclient/WebClient/DOESSearch.do#img_contract1";

        // Country Code
        public const string CountryCode_HK = "HK";

        // CarePack
        public const string WtyType_CarePack = "CarePack";

        // Out of Warranty
        public const string OutOfWty = "Out of Warranty";

        // SLA
        public const string SLA_Header = "Offers Related To Package Offer:";
        public const string SLA_Installation = "INSTALLATION";
        public const string SLA_HA101AC = "HA101AC";
        public const string SLA_DMR = "DMR/";

        // Config File Sheet Name
        public const string CS_TermCode = "TermCode";
        public const string CS_CarePackSLA = "CarePackSLA";
        public const string CS_SLA = "SLA";
        public const string CS_CarePackOfferCode = "CarePack Offer Code";
        public const string CS_SSNFORRTS = "SSN List For RTS";
        public const string CS_SSNFORADj = "SSN List For Adjustment";
        public const string CS_ProactivePackage = "Proactive Package";
        public const string CS_BillingPlanType = "Billing Plan Type";

        // Config File Path
        public const string ConfigFilePath = @"Resources\config\configFile.xlsx";
        public const string AnalysisResult = @"Resources\config\AnalysisResult.xlsx";
        //public const string DiscountForm = @"Resources\config\DiscountForm.xlsx";
        public const string DiscountForm = @"Resources\config\Discount Approval Form.xlsx";
        public const string HKRTSCalculation = @"Resources\config\HK RTS Calculation.xls";
        public const string EosForm = @"Resources\config\EOS Extension Form.xls";

        // Term Code Unit
        public const string TermCode_Unit_Year = "YEAR";
        public const string TermCode_Unit_Month = "MONTH";
        public const string TermCode_Unit_Day = "DAY";

        // clsFindInfo的Type
        public const string Find_Type_Qoute = "Qoute";
        public const string Find_Type_Base = "Base";
        public const string Find_Type_CarePack = "CarePack";
        public const string Find_Type_Contract = "Contract";

        // SMC
        public const string SMC_O = "O";
        public const string SMC_N = "N";
        public const string SMC_C = "C";
        public const string SMC_U = "U";

        // String
        public const string String_N = "N";
        public const string String_T = "T";
        public const string String_False = "False";
        public const string String_SUCESS = "SUCESS";
        public const string String_Blank = "Blank";
        public const string String_New = "New";
        public const string String_Update = "String_Update";

        // city
        public const string Country_HK = "HK";
        public const string Country_MO = "MO";

        public const string City_HK = "Hong Kong";
        public const string City_MO = "Macau";

        // FL
        public const string FL_StrIndicator = "ZCONV";
        public const string FL_PlanningPlant = "7300";
        public const string FL_FunctLocCat_H = "H";
        public const string FL_FunctLocCat_O = "O";
        public const string FL_FunctLocCat_X = "X";

        // PF
        public const string PF_AC = "AC";
        public const string PF_AE = "AE";
        public const string PF_AG = "AG";
        public const string PF_CE = "CE";
        public const string PF_HC = "HC";
        public const string PF_OC = "OC";
        public const string PF_SC = "SC";
        public const string PF_RE = "RE";
        public const string PF_BP = "RE";
        public const string PF_IC = "IC";
        public const string PF_PY = "RG";
        public const string PF_RG = "RG";
        public const string PF_R1 = "R1";
        public const string PF_SA = "SA";
        public const string PF_SM = "SM";
        public const string PF_SP = "AG";
        public const string PF_SR = "SR";
        public const string PF_RP = "RP";
        public const string PF_T2 = "RP";
        public const string PF_YG = "YG";
        public const string PF_Z9 = "Z9";
        public const string PF_ZQ = "ZQ";
        public const string PF_SS = "SS";

        // SS/SH
        public const string SSSH_HK = "NEW-HK";
        public const string SSSH_MO = "0043074187";

        // Ref Doc Type
        public const string Ref_Type_Q = "Q";
        public const string Ref_Type_C = "C";

        // Doc Text ID
        public const string Doc_Text_ZIC = "ZIC";
        public const string Doc_Text_ZHP = "ZHP";
        public const string Doc_Text_ZSPC = "ZSPC";
        public const string Doc_Text_ZISP = "ZISP";

        // Doc Text Obj
        public const string Doc_Text_OBJ_VBBK = "VBBK";
        public const string Doc_Text_OBJ_VBBP = "VBBP";

        // 通过AC Z9 特定值写 SS
        public const string AC_SS = "EN00455210";
        public const string Z9_SS = "SR000011";
    }
}
