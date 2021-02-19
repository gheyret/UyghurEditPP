/*
 * Created by SharpDevelop.
 * User: Gheyret Kenji
 * Date: 2020/11/12
 * Time: 8:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Description of Uyghur.
/// </summary>
public class Uyghur
{
	public const int UYGIN_HM = 0;
	public const int UYGIN_A  = 1;
	public const int UYGIN_E  = 2;
	public const int UYGIN_B  = 3;
	public const int UYGIN_P  = 4;
	public const int UYGIN_T  = 5;
	public const int UYGIN_J  = 6;
	public const int UYGIN_CH = 7;
	public const int UYGIN_X  = 8;
	public const int UYGIN_D  = 9;
	public const int UYGIN_R  = 10;
	public const int UYGIN_Z  = 11;
	public const int UYGIN_ZZ = 12;
	public const int UYGIN_S  = 13;
	public const int UYGIN_SH = 14;
	public const int UYGIN_GH = 15;
	public const int UYGIN_F  = 16;
	public const int UYGIN_Q  = 17;
	public const int UYGIN_K  = 18;
	public const int UYGIN_G  = 19;
	public const int UYGIN_NG = 20;
	public const int UYGIN_L  = 21;
	public const int UYGIN_M  = 22;
	public const int UYGIN_N  = 23;
	public const int UYGIN_H  = 24;
	public const int UYGIN_O  = 25;
	public const int UYGIN_U  = 26;
	public const int UYGIN_OO = 27;
	public const int UYGIN_UU = 28;
	public const int UYGIN_W  = 29;
	public const int UYGIN_EE = 30;
	public const int UYGIN_I  = 31;
	public const int UYGIN_Y  = 32;
	public const int UYGIN_LA = 33;
	
	// Uyghurce Unicode (kona yeziq heriplerning Unicodesi)
	//A
	public const char UYG_UN_A_6='\u0627';	//Yalghuz
	public const char UYG_UN_A_Y='\uFE8D';	//Yalghuz
	public const char UYG_UN_A_A='\uFE8E';	//Ayaq
	// E Heripi
	public const char UYG_UN_E_6='\u06D5';	//Yalghuz;
	public const char UYG_UN_E_Y='\uFEE9';	//Yalghuz;
	public const char UYG_UN_E_A='\uFEEA';	//Ayaq;
	// B
	public const char UYG_UN_B_6='\u0628';	//Yalghuz;
	public const char UYG_UN_B_Y='\uFE8F';	//Yalghuz;
	public const char UYG_UN_B_A='\uFE90';	//Ayaq;
	public const char UYG_UN_B_B='\uFE91';	//Bax;
	public const char UYG_UN_B_O='\uFE92';	//Ottura;
	//P
	public const char UYG_UN_P_6='\u067E';  //Yalghuz;
	public const char UYG_UN_P_Y='\uFB56';  //Yalghuz;
	public const char UYG_UN_P_A='\uFB57';	//Ayaq;
	public const char UYG_UN_P_B='\uFB58';	//Bax;
	public const char UYG_UN_P_O='\uFB59';	//Ottura;
	//T
	public const char UYG_UN_T_6='\u062A';  //Yalghuz;
	public const char UYG_UN_T_Y='\uFE95';  //Yalghuz;
	public const char UYG_UN_T_A='\uFE96';	//Ayaq;
	public const char UYG_UN_T_B='\uFE97';	//Bax;
	public const char UYG_UN_T_O='\uFE98';	//Ottura;
	//J
	public const char UYG_UN_J_6='\u062C';  //Yalghuz;
	public const char UYG_UN_J_Y='\uFE9D';  //Yalghuz;
	public const char UYG_UN_J_A='\uFE9E';	//Ayaq;
	public const char UYG_UN_J_B='\uFE9F';	//Bax;
	public const char UYG_UN_J_O='\uFEA0';	//Ottura;
	//Ch
	public const char UYG_UN_CH_6='\u0686';  //Yalghuz;
	public const char UYG_UN_CH_Y='\uFB7A';  //Yalghuz;
	public const char UYG_UN_CH_A='\uFB7B';	//Ayaq;
	public const char UYG_UN_CH_B='\uFB7C';	//Bax;
	public const char UYG_UN_CH_O='\uFB7D';	//Ottura;
	//X
	public const char UYG_UN_X_6='\u062E';  //Yalghuz;
	public const char UYG_UN_X_Y='\uFEA5'; //Yalghuz;
	public const char UYG_UN_X_A='\uFEA6';	//Ayaq;
	public const char UYG_UN_X_B='\uFEA7';	//Bax;
	public const char UYG_UN_X_O='\uFEA8';	//Ottura;
	//D
	public const char UYG_UN_D_6='\u062F';  //Yalghuz;
	public const char UYG_UN_D_Y='\uFEA9';  //Yalghuz;
	public const char UYG_UN_D_A='\uFEAA';	//Ayaq;
	//R
	public const char UYG_UN_R_6='\u0631';  //Yalghuz;
	public const char UYG_UN_R_Y='\uFEAD';  //Yalghuz;
	public const char UYG_UN_R_A='\uFEAE';	//Ayaq;
	//Z
	public const char UYG_UN_Z_6='\u0632';  //Yalghuz;
	public const char UYG_UN_Z_Y='\uFEAF';  //Yalghuz;
	public const char UYG_UN_Z_A='\uFEB0';	//Ayaq;
	//Z..
	public const char UYG_UN_ZZ_6='\u0698';  //Yalghuz;
	public const char UYG_UN_ZZ_Y='\uFB8A';  //Yalghuz;
	public const char UYG_UN_ZZ_A='\uFB8B';	//Ayaq;
	//S
	public const char UYG_UN_S_6='\u0633';  //Yalghuz;
	public const char UYG_UN_S_Y='\uFEB1';  //Yalghuz;
	public const char UYG_UN_S_A='\uFEB2';	//Ayaq;
	public const char UYG_UN_S_B='\uFEB3';	//Bax;
	public const char UYG_UN_S_O='\uFEB4';	//Ottura;
	//SH
	public const char UYG_UN_SH_6='\u0634';  //Yalghuz;
	public const char UYG_UN_SH_Y='\uFEB5';  //Yalghuz;
	public const char UYG_UN_SH_A='\uFEB6';	//Ayaq;
	public const char UYG_UN_SH_B='\uFEB7';	//Bax;
	public const char UYG_UN_SH_O='\uFEB8';	//Ottura;
	//GH
	public const char UYG_UN_GH_6='\u063A';  //Yalghuz;
	public const char UYG_UN_GH_Y='\uFECD';  //Yalghuz;
	public const char UYG_UN_GH_A='\uFECE';	//Ayaq;
	public const char UYG_UN_GH_B='\uFECF';	//Bax;
	public const char UYG_UN_GH_O='\uFED0';	//Ottura;
	//F
	public const char UYG_UN_F_6='\u0641';  //Yalghuz;
	public const char UYG_UN_F_Y='\uFED1';  //Yalghuz;
	public const char UYG_UN_F_A='\uFED2';	//Ayaq;
	public const char UYG_UN_F_B='\uFED3';	//Bax;
	public const char UYG_UN_F_O='\uFED4';	//Ottura;
	//Q
	public const char UYG_UN_Q_6='\u0642';  //Yalghuz;
	public const char UYG_UN_Q_Y='\uFED5';  //Yalghuz;
	public const char UYG_UN_Q_A='\uFED6';	//Ayaq;
	public const char UYG_UN_Q_B='\uFED7';	//Bax;
	public const char UYG_UN_Q_O='\uFED8';	//Ottura;
	//K
	public const char UYG_UN_K_6='\u0643';  //Yalghuz;
	public const char UYG_UN_K_Y='\uFED9';  //Yalghuz;
	public const char UYG_UN_K_A='\uFEDA';	//Ayaq;
	public const char UYG_UN_K_B='\uFEDB';	//Bax;
	public const char UYG_UN_K_O='\uFEDC';	//Ottura;
	//G
	public const char UYG_UN_G_6='\u06AF';  //Yalghuz;
	public const char UYG_UN_G_Y='\uFB92';  //Yalghuz;
	public const char UYG_UN_G_A='\uFB93';	//Ayaq;
	public const char UYG_UN_G_B='\uFB94';	//Bax;
	public const char UYG_UN_G_O='\uFB95';	//Ottura;
	//NG
	public const char UYG_UN_NG_6='\u06AD';  //Yalghuz;
	public const char UYG_UN_NG_Y='\uFBD3';  //Yalghuz;
	public const char UYG_UN_NG_A='\uFBD4';	//Ayaq;
	public const char UYG_UN_NG_B='\uFBD5';	//Bax;
	public const char UYG_UN_NG_O='\uFBD6';	//Ottura;
	//L
	public const char UYG_UN_L_6='\u0644';  //Yalghuz;
	public const char UYG_UN_L_Y='\uFEDD';  //Yalghuz;
	public const char UYG_UN_L_A='\uFEDE';	//Ayaq;
	public const char UYG_UN_L_B='\uFEDF';	//Bax;
	public const char UYG_UN_L_O='\uFEE0';	//Ottura;
	//M
	public const char UYG_UN_M_6='\u0645';  //Yalghuz;
	public const char UYG_UN_M_Y='\uFEE1';  //Yalghuz;
	public const char UYG_UN_M_A='\uFEE2';	//Ayaq;
	public const char UYG_UN_M_B='\uFEE3';	//Bax;
	public const char UYG_UN_M_O='\uFEE4';	//Ottura;
	//N
	public const char UYG_UN_N_6='\u0646';  //Yalghuz;
	public const char UYG_UN_N_Y='\uFEE5';  //Yalghuz;
	public const char UYG_UN_N_A='\uFEE6';	//Ayaq;
	public const char UYG_UN_N_B='\uFEE7';	//Bax;
	public const char UYG_UN_N_O='\uFEE8';	//Ottura;
	//H..
	public const char UYG_UN_H_6='\u06BE';  //Yalghuz;
	public const char UYG_UN_H_Y='\uFBAA';  //Yalghuz;
	public const char UYG_UN_H_A='\uFBAB';	//Ayaq;
	public const char UYG_UN_H_B='\uFBAC';	//Bax;
	public const char UYG_UN_H_O='\uFBAD';	//Ottura;

	//O
	public const char UYG_UN_O_6='\u0648';  //Yalghuz;
	public const char UYG_UN_O_Y='\uFEED';  //Yalghuz;
	public const char UYG_UN_O_A='\uFEEE';	//Ayaq;
	//U
	public const char UYG_UN_U_6='\u06C7';  //Yalghuz;
	public const char UYG_UN_U_Y='\uFBD7';  //Yalghuz;
	public const char UYG_UN_U_A='\uFBD8';	//Ayaq;
	//O..
	public const char UYG_UN_OO_6='\u06C6';  //Yalghuz;
	public const char UYG_UN_OO_Y='\uFBD9';  //Yalghuz;
	public const char UYG_UN_OO_A='\uFBDA';	//Ayaq;
	//U..
	public const char UYG_UN_UU_6='\u06C8';  //Yalghuz;
	public const char UYG_UN_UU_Y='\uFBDB';  //Yalghuz;
	public const char UYG_UN_UU_A='\uFBDC';	//Ayaq;
	//W
	public const char UYG_UN_W_6='\u06CB';  //Yalghuz;
	public const char UYG_UN_W_Y='\uFBDE'; //Yalghuz;
	public const char UYG_UN_W_A='\uFBDF';	//Ayaq;
	//E..
	public const char UYG_UN_EE_6='\u06D0';  //Yalghuz;
	public const char UYG_UN_EE_Y='\uFBE4';  //Yalghuz;
	public const char UYG_UN_EE_A='\uFBE5';	//Ayaq;
	public const char UYG_UN_EE_B='\uFBE6';	//Bax;
	public const char UYG_UN_EE_O='\uFBE7';	//Ottura;
	//I
	public const char UYG_UN_I_6='\u0649';  //Yalghuz;
	public const char UYG_UN_I_Y='\uFEEF';  //Yalghuz;
	public const char UYG_UN_I_A='\uFEF0';	//Ayaq;
	public const char UYG_UN_I_B='\uFBE8';	//Bax;
	public const char UYG_UN_I_O='\uFBE9';	//Ottura;
	//Y
	public const char UYG_UN_Y_6='\u064A';  //Yalghuz;
	public const char UYG_UN_Y_Y='\uFEF1';  //Yalghuz;
	public const char UYG_UN_Y_A='\uFEF2';	//Ayaq;
	public const char UYG_UN_Y_B='\uFEF3';	//Bax;
	public const char UYG_UN_Y_O='\uFEF4';	//Ottura;

	//La
	public const char UYG_UN_LA_6='\uFEFB';  //Yalghuz;
	public const char UYG_UN_LA_Y='\uFEFB';  //Yalghuz;
	public const char UYG_UN_LA_A='\uFEFC';	//Ayaq;

	//Amza
	public const char UYG_UN_HM_6='\u0626';  //Yalghuz;
	public const char UYG_UN_HM_Y='\uFE8B';  //Yalghuz;
	public const char UYG_UN_HM_O='\uFE8C';	//Ottura;

	// Towendikiler Uyghurche Heripler
	// Biraq ular KeyBord Arqiliq kirguzgende ishlitilmidi

	//hemze bilen A
	public const char UYGE_UN_HA_Y='\uFBEA';  //Yalghuz;
	public const char UYGE_UN_HA_A='\uFBEB';	//Ayaq;

	//hemze bilen E
	public const char UYGE_UN_HE_Y='\uFBEC';  //Yalghuz;
	public const char UYGE_UN_HE_A='\uFBED';	//Ayaq;

	//hemze bilen O
	public const char UYGE_UN_HO_Y='\uFBEE';  //Yalghuz;
	public const char UYGE_UN_HO_A='\uFBEF';	//Ayaq;

	//hemze bilen U
	public const char UYGE_UN_HU_Y='\uFBF0';  //Yalghuz;
	public const char UYGE_UN_HU_A='\uFBF1';  //Ayaq;

	//hemze bilen OO
	public const char UYGE_UN_HOO_Y='\uFBF2';  //Yalghuz;
	public const char UYGE_UN_HOO_A='\uFBF3';  //Ayaq;

	//hemze bilen UU
	public const char UYGE_UN_HUU_Y='\uFBF4';  //Yalghuz;
	public const char UYGE_UN_HUU_A='\uFBF5';  //Ayaq;

	//hemze bilen EE
	public const char UYGE_UN_HEE_Y='\uFBF6';  //Yalghuz;
	public const char UYGE_UN_HEE_A='\uFBF7';  //Ayaq;
	public const char UYGE_UN_HEE_B='\uFBF8';  //Ayaq;

	//hemze bilen I
	public const char UYGE_UN_HI_Y='\uFBF9';  //Yalghuz;
	public const char UYGE_UN_HI_A='\uFBFA';  //Ayaq;
	public const char UYGE_UN_HI_B='\uFBFB';  //Ayaq;
	
	public const char UYG_UN_PESH       = '\u060C'; //Pesh
	public const char UYG_UN_SOZUSH     = '\u0640'; //Sozlerni tengshesh belgisi
	public const char UYG_UN_CHEKITPESH = '\u061B'; // Chekitlik Pesh
	public const char UYG_UN_CHONG      = '\u203A'; // ChongBelgisi
	public const char UYG_UN_KICHIK     = '\u2039'; // Kichik Belgisi
	public const char UYG_UN_SOAL       = '\u061F'; // Soal Belgisi
	public const char UYG_UN_KOPCHEKIT  = '\u2026'; // kop chekit
	public const char UYG_UN_SIZIQ      = '\u2014'; // Siziq
	public const char UYG_UN_SOL_QOSH_TIRNAQ='\u00AB'; // Qosh Tirnaq<<
	public const char UYG_UN_ONG_QOSH_TIRNAQ='\u00BB'; // Qosh Tirnaq>>
	public const char UYG_UN_YULTUZ      ='\u066D'; //Yultuz belgisi
	
	public enum YEZIQ{UEY,ULY,USY,YOQ};
	
	
	public Uyghur()
	{
		System.Diagnostics.Debug.WriteLine("Keldi");
	}
	
	public static String Sozghuch{
		get{
			return UYG_UN_SOZUSH+"";
		}
	}
	
	public static string UEYHerpler{
		get{
			return "ـئابتجخدرزسشغفقكلمنوىيپچژڭگھۆۇۈۋېەلا";
		}
	}
	
	public static string ULYHerpler{
		get{
			return "ABCDEFGHIJKLMNOPQRSTUWXYZÉÖÜabcdefghijklmnopqrstuwxyzéöü’'";
		}
	}
	public static string USYHerpler{
		get{
			return "АБВГДЕЖЗИЙКЛМНОПРСТУФХЧШҒҖҚҢҮҺӘӨабвгдежзийклмнопрстуфхчшғҗқңүһәөъ’'";
		}
	}
	
	
	public static string ULYJumleChongYaz(string newtext)
	{
		string qur;
		Regex jumle = new Regex(@"([\.\?!][\'\""\u2018\u2019\u201c\u201d\)\]]*\s*(?<!\w\.\w.)(?<![A-ZÉÖÜ][a-z][a-z]\.)(?<![A-ZÉÖÜ][a-z]\.)(?<![A-ZÉÖÜ]\.)\s+)",RegexOptions.Compiled);
		string[] sp = {"\r\n"};
		string[] qurlar = newtext.Split(sp,StringSplitOptions.None);
		for(int i=0;i<qurlar.Length;i++){
			if(qurlar[i].Length>0){
				qur = qurlar[i];
				int stind = 0;
				Match match = jumle.Match(qur, stind);
				char herp;
				while(match.Success){
					stind = match.Index;
					herp = qur[stind];
					while(Uyghur.ULYHerpler.IndexOf(herp)==-1){
						stind++;
						if(stind>=qur.Length){
							break;
						}
						herp = qur[stind];
					}
					if(stind<qur.Length){
						qur = qur.Remove(stind,1).Insert(stind,char.ToUpper(herp).ToString());
					}
					match = jumle.Match(qur, stind);
				}
				stind =0;
				herp  = qur[stind];
				while(Uyghur.ULYHerpler.IndexOf(herp)==-1){
					stind++;
					if(stind>=qur.Length){
						break;
					}
					herp = qur[stind];
				}
				if(stind<qur.Length && char.IsLower(qur[stind])){
					qur = qur.Remove(stind,1).Insert(stind,char.ToUpper(herp).ToString());
				}
				qurlar[i] = qur;
			}
		}
		return string.Join(sp[0],qurlar);
	}
	

	public static string USYJumleChongYaz(string newtext)
	{
		string qur;
		Regex jumle = new Regex(@"([\.\?!][\'\""\u2018\u2019\u201c\u201d\)\]]*\s*(?<!\w\.\w.)(?<![АБВГДЕЖЗИЙКЛМНОПРСТУФХЧШҒҖҚҢҮҺӘӨ][абвгдежзийклмнопрстуфхчшғҗқңүһәө][абвгдежзийклмнопрстуфхчшғҗқңүһәө]\.)(?<![АБВГДЕЖЗИЙКЛМНОПРСТУФХЧШҒҖҚҢҮҺӘӨ][абвгдежзийклмнопрстуфхчшғҗқңүһәө]\.)(?<![АБВГДЕЖЗИЙКЛМНОПРСТУФХЧШҒҖҚҢҮҺӘӨ]\.)\s+)",RegexOptions.Compiled);
		string[] sp = {"\r\n"};
		string[] qurlar = newtext.Split(sp,StringSplitOptions.None);
		for(int i=0;i<qurlar.Length;i++){
			if(qurlar[i].Length>0){
				qur = qurlar[i];
				int stind = 0;
				Match match = jumle.Match(qur, stind);
				char herp;
				while(match.Success){
					stind = match.Index;
					herp = qur[stind];
					while(Uyghur.USYHerpler.IndexOf(herp)==-1){
						stind++;
						if(stind>=qur.Length){
							break;
						}
						herp = qur[stind];
					}
					if(stind<qur.Length){
						qur = qur.Remove(stind,1).Insert(stind,char.ToUpper(herp).ToString());
					}
					match = jumle.Match(qur, stind);
				}
				stind =0;
				herp  = qur[stind];
				while(Uyghur.USYHerpler.IndexOf(herp)==-1){
					stind++;
					if(stind>=qur.Length){
						break;
					}
					herp = qur[stind];
				}
				if(stind<qur.Length && char.IsLower(qur[stind])){
					qur = qur.Remove(stind,1).Insert(stind,char.ToUpper(herp).ToString());
				}
				qurlar[i] = qur;
			}
		}
		return string.Join(sp[0],qurlar);
	}

	public static bool IsUyghurcheHerp(char Herp)
	{
		bool ret=false;
		switch(Herp)
		{
			case  UYG_UN_A_6  :
			case  UYG_UN_E_6  :
			case  UYG_UN_B_6  :
			case  UYG_UN_P_6 :
			case  UYG_UN_T_6  :
			case  UYG_UN_J_6  :
			case  UYG_UN_CH_6 :
			case  UYG_UN_X_6 :
			case  UYG_UN_D_6  :
			case  UYG_UN_R_6 :
			case  UYG_UN_Z_6 :
			case  UYG_UN_ZZ_6 :
			case  UYG_UN_S_6 :
			case  UYG_UN_SH_6 :
			case  UYG_UN_GH_6 :
			case  UYG_UN_F_6  :
			case  UYG_UN_Q_6 :
			case  UYG_UN_K_6 :
			case  UYG_UN_G_6  :
			case  UYG_UN_NG_6  :
			case  UYG_UN_L_6  :
			case  UYG_UN_M_6  :
			case  UYG_UN_N_6 :
			case  UYG_UN_H_6  :
			case  UYG_UN_O_6  :
			case  UYG_UN_U_6  :
			case  UYG_UN_OO_6 :
			case  UYG_UN_UU_6 :
			case  UYG_UN_W_6 :
			case  UYG_UN_EE_6 :
			case  UYG_UN_I_6  :
			case  UYG_UN_Y_6  :
			case  UYG_UN_HM_6 :
				ret=true;
				break;
			default:
				ret=false;
				break;
		}
		return ret;
	}


	public static bool IsUSozuq(char Herp)
	{
		bool ret=false;
		switch(Herp)
		{
			case  UYG_UN_A_6  :
			case  UYG_UN_E_6  :
			case  UYG_UN_O_6  :
			case  UYG_UN_U_6  :
			case  UYG_UN_OO_6 :
			case  UYG_UN_UU_6 :
			case  UYG_UN_EE_6 :
			case  UYG_UN_I_6  :
				ret=true;
				break;
			default:
				ret=false;
				break;
		}
		return ret;
	}
	
	public static bool IsSozuq(char Herp)
	{
		bool ret=false;
		switch(Herp)
		{
			case  UYG_UN_A_6  :
			case  UYG_UN_E_6  :
			case  UYG_UN_O_6  :
			case  UYG_UN_U_6  :
			case  UYG_UN_OO_6 :
			case  UYG_UN_UU_6 :
			case  UYG_UN_EE_6 :
			case  UYG_UN_I_6  :
			case  'a':
			case  'e'  :
			case  'o'  :
			case  'u'  :
			case  'i' :
			case  'ö' :
			case  'ü' :
			case  'é':
			case  'а':
			case  'ә'  :
			case  'о'  :
			case  'у'  :
			case  'и' :
			case  'ө' :
			case  'ү' :
			case  'е':
				ret=true;
				break;
			default:
				ret=false;
				break;
		}
		return ret;
	}
	
	
	public static int GetUnicodeIndex(char Herp)
	{
		int ret=-1;
		switch(Herp)
		{
			case  UYG_UN_A_6:
			case  UYG_UN_A_Y:
			case  UYG_UN_A_A:
				ret=UYGIN_A ;
				break;
			case  UYG_UN_E_6:
			case  UYG_UN_E_Y:
			case  UYG_UN_E_A:
				ret=UYGIN_E ;
				break;
			case  UYG_UN_B_6:
			case  UYG_UN_B_Y:
			case  UYG_UN_B_B:
			case  UYG_UN_B_O:
			case  UYG_UN_B_A:
				ret=UYGIN_B ;
				break;
			case  UYG_UN_P_6:
			case  UYG_UN_P_Y:
			case  UYG_UN_P_B:
			case  UYG_UN_P_O:
			case  UYG_UN_P_A:
				ret=UYGIN_P;
				break;
			case  UYG_UN_T_6:
			case  UYG_UN_T_Y:
			case  UYG_UN_T_B:
			case  UYG_UN_T_O:
			case  UYG_UN_T_A:
				ret=UYGIN_T ;
				break;
			case  UYG_UN_J_6:
			case  UYG_UN_J_Y:
			case  UYG_UN_J_B:
			case  UYG_UN_J_O:
			case  UYG_UN_J_A:
				ret=UYGIN_J ;
				break;
			case  UYG_UN_CH_6:
			case  UYG_UN_CH_Y:
			case  UYG_UN_CH_B:
			case  UYG_UN_CH_O:
			case  UYG_UN_CH_A:
				ret=UYGIN_CH;
				break;
			case  UYG_UN_X_6:
			case  UYG_UN_X_Y:
			case  UYG_UN_X_B:
			case  UYG_UN_X_O:
			case  UYG_UN_X_A:
				ret=UYGIN_X;
				break;
			case  UYG_UN_D_6:
			case  UYG_UN_D_Y:
			case  UYG_UN_D_A:
				ret=UYGIN_D ;
				break;
			case  UYG_UN_R_6:
			case  UYG_UN_R_Y:
			case  UYG_UN_R_A:
				ret=UYGIN_R;
				break;
			case  UYG_UN_Z_6:
			case  UYG_UN_Z_Y:
			case  UYG_UN_Z_A:
				ret=UYGIN_Z;
				break;
			case  UYG_UN_ZZ_6:
			case  UYG_UN_ZZ_Y:
			case  UYG_UN_ZZ_A:
				ret=UYGIN_ZZ;
				break;
			case  UYG_UN_S_6:
			case  UYG_UN_S_Y:
			case  UYG_UN_S_B:
			case  UYG_UN_S_O:
			case  UYG_UN_S_A:
				ret=UYGIN_S;
				break;
			case  UYG_UN_SH_6:
			case  UYG_UN_SH_Y:
			case  UYG_UN_SH_B:
			case  UYG_UN_SH_O:
			case  UYG_UN_SH_A:
				ret=UYGIN_SH;
				break;
			case  UYG_UN_GH_6:
			case  UYG_UN_GH_Y:
			case  UYG_UN_GH_B:
			case  UYG_UN_GH_O:
			case  UYG_UN_GH_A:
				ret=UYGIN_GH;
				break;
			case  UYG_UN_F_6:
			case  UYG_UN_F_Y:
			case  UYG_UN_F_B:
			case  UYG_UN_F_O:
			case  UYG_UN_F_A:
				ret=UYGIN_F ;
				break;
			case  UYG_UN_Q_6:
			case  UYG_UN_Q_Y:
			case  UYG_UN_Q_B:
			case  UYG_UN_Q_O:
			case  UYG_UN_Q_A:
				ret=UYGIN_Q;
				break;
			case  UYG_UN_K_6:
			case  UYG_UN_K_Y:
			case  UYG_UN_K_B:
			case  UYG_UN_K_O:
			case  UYG_UN_K_A:
				ret=UYGIN_K;
				break;
			case  UYG_UN_G_6:
			case  UYG_UN_G_Y:
			case  UYG_UN_G_B:
			case  UYG_UN_G_O:
			case  UYG_UN_G_A:
				ret=UYGIN_G ;
				break;
			case  UYG_UN_NG_6:
			case  UYG_UN_NG_Y:
			case  UYG_UN_NG_B:
			case  UYG_UN_NG_O:
			case  UYG_UN_NG_A:
				ret=UYGIN_NG ;
				break;
			case  UYG_UN_L_6:
			case  UYG_UN_L_Y:
			case  UYG_UN_L_B:
			case  UYG_UN_L_O:
			case  UYG_UN_L_A:
				ret=UYGIN_L ;
				break;
			case  UYG_UN_M_6:
			case  UYG_UN_M_Y:
			case  UYG_UN_M_B:
			case  UYG_UN_M_O:
			case  UYG_UN_M_A:
				ret=UYGIN_M ;
				break;
			case  UYG_UN_N_6:
			case  UYG_UN_N_Y:
			case  UYG_UN_N_B:
			case  UYG_UN_N_O:
			case  UYG_UN_N_A:
				ret=UYGIN_N;
				break;
			case  UYG_UN_H_6:
			case  UYG_UN_H_Y:
			case  UYG_UN_H_A:
			case  UYG_UN_H_B:
			case  UYG_UN_H_O:
				//case  UYGE_UN_H_Y:   // Yulghunda Ishlitilgen
				//case  UYGE_UN_H_A:   // Yulghunda ishlitilgen
				ret=UYGIN_H ;
				break;
			case  UYG_UN_O_6:
			case  UYG_UN_O_Y:
			case  UYG_UN_O_A:
				ret=UYGIN_O ;
				break;
			case  UYG_UN_U_6:
			case  UYG_UN_U_Y:
			case  UYG_UN_U_A:
				ret=UYGIN_U ;
				break;
			case  UYG_UN_OO_6:
			case  UYG_UN_OO_Y:
			case  UYG_UN_OO_A:
				ret=UYGIN_OO;
				break;
			case  UYG_UN_UU_6:
			case  UYG_UN_UU_Y:
			case  UYG_UN_UU_A:
				ret=UYGIN_UU;
				break;
			case  UYG_UN_W_6:
			case  UYG_UN_W_Y:
			case  UYG_UN_W_A:
				ret=UYGIN_W;
				break;
			case  UYG_UN_EE_6:
			case  UYG_UN_EE_Y:
			case  UYG_UN_EE_B:
			case  UYG_UN_EE_O:
			case  UYG_UN_EE_A:
				ret=UYGIN_EE;
				break;
			case  UYG_UN_I_6:
			case  UYG_UN_I_Y:
			case  UYG_UN_I_B:
			case  UYG_UN_I_O:
			case  UYG_UN_I_A:
				ret=UYGIN_I ;
				break;
			case  UYG_UN_Y_6:
			case  UYG_UN_Y_Y:
			case  UYG_UN_Y_B:
			case  UYG_UN_Y_O:
			case  UYG_UN_Y_A:
				ret=UYGIN_Y ;
				break;
			case  UYG_UN_LA_Y:
			case  UYG_UN_LA_A:
				ret=UYGIN_LA;
				break;
			case  UYG_UN_HM_6:
			case  UYG_UN_HM_Y:
			case  UYG_UN_HM_O:
				ret=UYGIN_HM;
				break;
			default:
				break;

		}
		return ret;
	}
	
	
	

	public static String UEY2ULY(String uyghurkona)
	{
//			int stopChange=0;
		StringBuilder token=new StringBuilder();
		StringBuilder unicode=new StringBuilder();
		int len=0;
		int pos=0;
		uyghurkona = uyghurkona.Replace(Uyghur.Sozghuch,"");
		int size=uyghurkona.Length;
		if(size==0) return "";
		char Herp;
		while(pos<size)
		{
			Herp=uyghurkona[pos];
//				if(Herp=='<')
//				{
//					stopChange++;
//					unicode.Append(Herp);
//					pos++;
//					continue;
//				}
//				else if(Herp=='>')
//				{
//					stopChange--;
//					if(stopChange<0)stopChange=0;
//					unicode.Append(Herp);
//					pos++;
//					continue;
//				}
//
//				if(stopChange!=0)
//				{
//					unicode.Append(Herp);
//					pos++;
//					continue;
//				}

			if(IsUyghurcheHerp(Herp)==true || Herp=='&')
			{
				token.Length=0;
				if(IsUyghurcheHerp(Herp)==true)
				{
					while(IsUyghurcheHerp(Herp)==true)
					{
						token.Append(Herp);
						pos++;
						if(pos>=size) break;
						Herp=uyghurkona[pos];
					}
				}
				else
				{
					while(Herp!=';' && Herp!='<' && Herp!='>' && Herp!=' '  && Herp!='\n' && Herp!='\r')
					{
						if(Herp>=' ' && Herp<='~')
						{
							token.Append(Herp);
							pos++;
							if(pos>=size) break;
							Herp=uyghurkona[pos];
						}
						else
						{
							break;
						}
					}
					if(Herp==';'){
						token.Append(Herp);
						pos++;
					}
				}
				len=token.Length;
				if(len>0)
				{
					if(IsConstanToken(token)==true)
					{
						unicode.Append(U2LSBelge(token.ToString()));
					}
					else
					{
						unicode.Append(U2L(token));
					}
				}
			}
			else
			{
				unicode.Append(U2LSBelge(Herp+""));
				pos++;
			}
		}
		return unicode.ToString();
	}


	public static String UEY2USY(String uyghurkona)
	{
//			int stopChange=0;
		StringBuilder token=new StringBuilder();
		StringBuilder unicode=new StringBuilder();
		int len=0;
		int pos=0;
		uyghurkona = uyghurkona.Replace(Uyghur.Sozghuch,"");
		int size=uyghurkona.Length;
		if(size==0) return "";
		char Herp;
		while(pos<size)
		{
			Herp=uyghurkona[pos];
//				if(Herp=='<')
//				{
//					stopChange++;
//					unicode.Append(Herp);
//					pos++;
//					continue;
//				}
//				else if(Herp=='>')
//				{
//					stopChange--;
//					if(stopChange<0)stopChange=0;
//					unicode.Append(Herp);
//					pos++;
//					continue;
//				}
//
//				if(stopChange!=0)
//				{
//					unicode.Append(Herp);
//					pos++;
//					continue;
//				}

			if(IsUyghurcheHerp(Herp)==true || Herp=='&')
			{
				token.Length=0;
				if(IsUyghurcheHerp(Herp)==true)
				{
					while(IsUyghurcheHerp(Herp)==true)
					{
						token.Append(Herp);
						pos++;
						if(pos>=size) break;
						Herp=uyghurkona[pos];
					}
				}
				else
				{
					while(Herp!=';' && Herp!='<' && Herp!='>' && Herp!=' '  && Herp!='\n' && Herp!='\r')
					{
						if(Herp>=' ' && Herp<='~')
						{
							token.Append(Herp);
							pos++;
							if(pos>=size) break;
							Herp=uyghurkona[pos];
						}
						else
						{
							break;
						}
					}
					if(Herp==';'){
						token.Append(Herp);
						pos++;
					}
				}
				len=token.Length;
				if(len>0)
				{
					if(IsConstanToken(token)==true)
					{
						unicode.Append(U2LSBelge(token.ToString()));
					}
					else
					{
						unicode.Append(U2S(token));
					}
				}
			}
			else
			{
				unicode.Append(U2LSBelge(Herp+""));
				pos++;
			}
		}
		return unicode.ToString();
	}


	static bool IsConstanToken(StringBuilder token)
	{
		bool ret=false;
		int size=token.Length;
		if(token[0]=='&' && token[size-1]==';')
		{
			ret=true;
		}
		return ret;
	}

	static String U2L(StringBuilder token)
	{
		bool aldiN=false;
		bool skip=true;
		StringBuilder uly=new StringBuilder();
		foreach(char Herp in token.ToString())
		{
			switch(Herp)
			{
				case  UYG_UN_HM_6:
				case  UYG_UN_HM_Y:
				case  UYG_UN_HM_O:
					aldiN=false;
					if(skip==true)
					{
					}
					else
					{
						uly.Append("’");
					}
					break;
				case  UYG_UN_A_6:
				case  UYG_UN_A_Y:
				case  UYG_UN_A_A:
					aldiN=false;
					skip=false;
					uly.Append("a");
					break;
				case  UYG_UN_E_6:
				case  UYG_UN_E_Y:
				case  UYG_UN_E_A:
					aldiN=false;
					skip=false;
					uly.Append("e");
					break;
				case  UYG_UN_B_6:
				case  UYG_UN_B_Y:
				case  UYG_UN_B_B:
				case  UYG_UN_B_O:
				case  UYG_UN_B_A:
					aldiN=false;
					skip=false;
					uly.Append("b");
					break;
				case  UYG_UN_P_6:
				case  UYG_UN_P_Y:
				case  UYG_UN_P_B:
				case  UYG_UN_P_O:
				case  UYG_UN_P_A:
					aldiN=false;
					skip=false;
					uly.Append("p");
					break;
				case  UYG_UN_T_6:
				case  UYG_UN_T_Y:
				case  UYG_UN_T_B:
				case  UYG_UN_T_O:
				case  UYG_UN_T_A:
					aldiN=false;
					skip=false;
					uly.Append("t");
					break;
				case  UYG_UN_J_6:
				case  UYG_UN_J_Y:
				case  UYG_UN_J_B:
				case  UYG_UN_J_O:
				case  UYG_UN_J_A:
					aldiN=false;
					skip=false;
					uly.Append("j");
					break;
				case  UYG_UN_CH_6:
				case  UYG_UN_CH_Y:
				case  UYG_UN_CH_B:
				case  UYG_UN_CH_O:
				case  UYG_UN_CH_A:
					aldiN=false;
					skip=false;
					uly.Append("ch");
					break;
				case  UYG_UN_X_6:
				case  UYG_UN_X_Y:
				case  UYG_UN_X_B:
				case  UYG_UN_X_O:
				case  UYG_UN_X_A:
					aldiN=false;
					skip=false;
					uly.Append("x");
					break;
				case  UYG_UN_D_6:
				case  UYG_UN_D_Y:
				case  UYG_UN_D_A:
					aldiN=false;
					skip=false;
					uly.Append("d");
					break;
				case  UYG_UN_R_6:
				case  UYG_UN_R_Y:
				case  UYG_UN_R_A:
					aldiN=false;
					skip=false;
					uly.Append("r");
					break;
				case  UYG_UN_Z_6:
				case  UYG_UN_Z_Y:
				case  UYG_UN_Z_A:
					aldiN=false;
					skip=false;
					uly.Append("z");
					break;
				case  UYG_UN_ZZ_6:
				case  UYG_UN_ZZ_Y:
				case  UYG_UN_ZZ_A:
					aldiN=false;
					skip=false;
					uly.Append("zh");
					break;
				case  UYG_UN_S_6:
				case  UYG_UN_S_Y:
				case  UYG_UN_S_B:
				case  UYG_UN_S_O:
				case  UYG_UN_S_A:
					aldiN=false;
					skip=false;
					uly.Append("s");
					break;
				case  UYG_UN_SH_6:
				case  UYG_UN_SH_Y:
				case  UYG_UN_SH_B:
				case  UYG_UN_SH_O:
				case  UYG_UN_SH_A:
					aldiN=false;
					skip=false;
					uly.Append("sh");
					break;
				case  UYG_UN_GH_6:
				case  UYG_UN_GH_Y:
				case  UYG_UN_GH_B:
				case  UYG_UN_GH_O:
				case  UYG_UN_GH_A:
					skip=false;
					if(aldiN==true)
					{
						uly.Append("’gh");
					}
					else
					{
						uly.Append("gh");
					}
					aldiN=false;
					break;
				case  UYG_UN_F_6:
				case  UYG_UN_F_Y:
				case  UYG_UN_F_B:
				case  UYG_UN_F_O:
				case  UYG_UN_F_A:
					uly.Append("f");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_Q_6:
				case  UYG_UN_Q_Y:
				case  UYG_UN_Q_B:
				case  UYG_UN_Q_O:
				case  UYG_UN_Q_A:
					uly.Append("q");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_K_6:
				case  UYG_UN_K_Y:
				case  UYG_UN_K_B:
				case  UYG_UN_K_O:
				case  UYG_UN_K_A:
					uly.Append("k");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_G_6:
				case  UYG_UN_G_Y:
				case  UYG_UN_G_B:
				case  UYG_UN_G_O:
				case  UYG_UN_G_A:
					if(aldiN==true)
					{
						uly.Append("’g");
					}
					else
					{
						uly.Append("g");
					}
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_NG_6:
				case  UYG_UN_NG_Y:
				case  UYG_UN_NG_B:
				case  UYG_UN_NG_O:
				case  UYG_UN_NG_A:
					uly.Append("ng");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_L_6:
				case  UYG_UN_L_Y:
				case  UYG_UN_L_B:
				case  UYG_UN_L_O:
				case  UYG_UN_L_A:
					uly.Append("l");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_M_6:
				case  UYG_UN_M_Y:
				case  UYG_UN_M_B:
				case  UYG_UN_M_O:
				case  UYG_UN_M_A:
					uly.Append("m");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_N_6:
				case  UYG_UN_N_Y:
				case  UYG_UN_N_B:
				case  UYG_UN_N_O:
				case  UYG_UN_N_A:
					uly.Append("n");
					skip=false;
					aldiN=true;
					break;
				case  UYG_UN_H_6:
				case  UYG_UN_H_Y:
				case  UYG_UN_H_A:
				case  UYG_UN_H_B:
				case  UYG_UN_H_O:
					//case  UYGE_UN_H_Y:   // Yulghunda Ishlitilgen
					//case  UYGE_UN_H_A:   // Yulghunda ishlitilgen
					uly.Append("h");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_O_6:
				case  UYG_UN_O_Y:
				case  UYG_UN_O_A:
					uly.Append("o");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_U_6:
				case  UYG_UN_U_Y:
				case  UYG_UN_U_A:
					uly.Append("u");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_OO_6:
				case  UYG_UN_OO_Y:
				case  UYG_UN_OO_A:
					uly.Append("ö");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_UU_6:
				case  UYG_UN_UU_Y:
				case  UYG_UN_UU_A:
					uly.Append("ü");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_W_6:
				case  UYG_UN_W_Y:
				case  UYG_UN_W_A:
					uly.Append("w");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_EE_6:
				case  UYG_UN_EE_Y:
				case  UYG_UN_EE_B:
				case  UYG_UN_EE_O:
				case  UYG_UN_EE_A:
					uly.Append("é");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_I_6:
				case  UYG_UN_I_Y:
				case  UYG_UN_I_B:
				case  UYG_UN_I_O:
				case  UYG_UN_I_A:
					uly.Append("i");
					skip=false;
					aldiN=false;
					break;
				case  UYG_UN_Y_6:
				case  UYG_UN_Y_Y:
				case  UYG_UN_Y_B:
				case  UYG_UN_Y_O:
				case  UYG_UN_Y_A:
					uly.Append("y");
					skip=false;
					aldiN=false;
					break;
				default:
					skip=true;
					uly.Append(U2LSBelge(Herp+""));
					aldiN=false;
					break;
			}
		}
		return uly.ToString();
	}


	static String U2S(StringBuilder token)
	{
		bool skip=true;
		StringBuilder uly=new StringBuilder();
		foreach(char Herp in token.ToString())
		{
			switch(Herp)
			{
				case  UYG_UN_HM_6:
				case  UYG_UN_HM_Y:
				case  UYG_UN_HM_O:
					if(skip==true)
					{
					}
					else
					{
						uly.Append('ъ');
					}
					continue;
				case  UYG_UN_A_6:
				case  UYG_UN_A_Y:
				case  UYG_UN_A_A:
					skip=false;
					uly.Append('а');//а*/
					break;
				case  UYG_UN_E_6:
				case  UYG_UN_E_Y:
				case  UYG_UN_E_A:
					skip=false;
					uly.Append('ә');//ә*/
					break;
				case  UYG_UN_B_6:
				case  UYG_UN_B_Y:
				case  UYG_UN_B_B:
				case  UYG_UN_B_O:
				case  UYG_UN_B_A:
					skip=false;
					uly.Append('б');//б*/
					break;
				case  UYG_UN_P_6:
				case  UYG_UN_P_Y:
				case  UYG_UN_P_B:
				case  UYG_UN_P_O:
				case  UYG_UN_P_A:
					skip=false;
					uly.Append('п');//п*/
					break;
				case  UYG_UN_T_6:
				case  UYG_UN_T_Y:
				case  UYG_UN_T_B:
				case  UYG_UN_T_O:
				case  UYG_UN_T_A:
					skip=false;
					uly.Append('т');//т*/
					break;
				case  UYG_UN_J_6:
				case  UYG_UN_J_Y:
				case  UYG_UN_J_B:
				case  UYG_UN_J_O:
				case  UYG_UN_J_A:
					skip=false;
					uly.Append('җ');//җ*/
					break;
				case  UYG_UN_CH_6:
				case  UYG_UN_CH_Y:
				case  UYG_UN_CH_B:
				case  UYG_UN_CH_O:
				case  UYG_UN_CH_A:
					skip=false;
					uly.Append('ч');//ч*/
					break;
				case  UYG_UN_X_6:
				case  UYG_UN_X_Y:
				case  UYG_UN_X_B:
				case  UYG_UN_X_O:
				case  UYG_UN_X_A:
					skip=false;
					uly.Append('х');//х*/
					break;
				case  UYG_UN_D_6:
				case  UYG_UN_D_Y:
				case  UYG_UN_D_A:
					skip=false;
					uly.Append('д');//д*/
					break;
				case  UYG_UN_R_6:
				case  UYG_UN_R_Y:
				case  UYG_UN_R_A:
					skip=false;
					uly.Append('р');//р*/
					break;
				case  UYG_UN_Z_6:
				case  UYG_UN_Z_Y:
				case  UYG_UN_Z_A:
					skip=false;
					uly.Append('з');//з*/
					break;
				case  UYG_UN_ZZ_6:
				case  UYG_UN_ZZ_Y:
				case  UYG_UN_ZZ_A:
					skip=false;
					uly.Append('ж');//ж*/
					break;
				case  UYG_UN_S_6:
				case  UYG_UN_S_Y:
				case  UYG_UN_S_B:
				case  UYG_UN_S_O:
				case  UYG_UN_S_A:
					skip=false;
					uly.Append('с');//с*/
					break;
				case  UYG_UN_SH_6:
				case  UYG_UN_SH_Y:
				case  UYG_UN_SH_B:
				case  UYG_UN_SH_O:
				case  UYG_UN_SH_A:
					skip=false;
					uly.Append('ш');//ш*/
					break;
				case  UYG_UN_GH_6:
				case  UYG_UN_GH_Y:
				case  UYG_UN_GH_B:
				case  UYG_UN_GH_O:
				case  UYG_UN_GH_A:
					skip=false;
					uly.Append('ғ');//ғ*/
					break;
				case  UYG_UN_F_6:
				case  UYG_UN_F_Y:
				case  UYG_UN_F_B:
				case  UYG_UN_F_O:
				case  UYG_UN_F_A:
					skip=false;
					uly.Append('ф');//ф*/
					break;
				case  UYG_UN_Q_6:
				case  UYG_UN_Q_Y:
				case  UYG_UN_Q_B:
				case  UYG_UN_Q_O:
				case  UYG_UN_Q_A:
					skip=false;
					uly.Append('қ');//қ*/
					break;
				case  UYG_UN_K_6:
				case  UYG_UN_K_Y:
				case  UYG_UN_K_B:
				case  UYG_UN_K_O:
				case  UYG_UN_K_A:
					skip=false;
					uly.Append('к');//к*/
					break;
				case  UYG_UN_G_6:
				case  UYG_UN_G_Y:
				case  UYG_UN_G_B:
				case  UYG_UN_G_O:
				case  UYG_UN_G_A:
					skip=false;
					uly.Append('г');//г*/
					break;
				case  UYG_UN_NG_6:
				case  UYG_UN_NG_Y:
				case  UYG_UN_NG_B:
				case  UYG_UN_NG_O:
				case  UYG_UN_NG_A:
					skip=false;
					uly.Append('ң');//ң*/
					break;
				case  UYG_UN_L_6:
				case  UYG_UN_L_Y:
				case  UYG_UN_L_B:
				case  UYG_UN_L_O:
				case  UYG_UN_L_A:
					skip=false;
					uly.Append('л');//л*/
					break;
				case  UYG_UN_M_6:
				case  UYG_UN_M_Y:
				case  UYG_UN_M_B:
				case  UYG_UN_M_O:
				case  UYG_UN_M_A:
					skip=false;
					uly.Append('м');//м*/
					break;
				case  UYG_UN_N_6:
				case  UYG_UN_N_Y:
				case  UYG_UN_N_B:
				case  UYG_UN_N_O:
				case  UYG_UN_N_A:
					skip=false;
					uly.Append('н');//н*/
					break;
				case  UYG_UN_H_6:
				case  UYG_UN_H_Y:
				case  UYG_UN_H_A:
				case  UYG_UN_H_B:
				case  UYG_UN_H_O:
					//case  UYGE_UN_H_Y:   // Yulghunda Ishlitilgen
					//case  UYGE_UN_H_A:   // Yulghunda ishlitilgen
					skip=false;
					uly.Append('һ');//һ*/
					break;
				case  UYG_UN_O_6:
				case  UYG_UN_O_Y:
				case  UYG_UN_O_A:
					skip=false;
					uly.Append('о');//о*/
					break;
				case  UYG_UN_U_6:
				case  UYG_UN_U_Y:
				case  UYG_UN_U_A:
					skip=false;
					uly.Append('у');//у*/
					break;
				case  UYG_UN_OO_6:
				case  UYG_UN_OO_Y:
				case  UYG_UN_OO_A:
					skip=false;
					uly.Append('ө');//ө*/
					break;
				case  UYG_UN_UU_6:
				case  UYG_UN_UU_Y:
				case  UYG_UN_UU_A:
					skip=false;
					uly.Append('ү');//ү*/
					break;
				case  UYG_UN_W_6:
				case  UYG_UN_W_Y:
				case  UYG_UN_W_A:
					skip=false;
					uly.Append('в');//в*/
					break;
				case  UYG_UN_EE_6:
				case  UYG_UN_EE_Y:
				case  UYG_UN_EE_B:
				case  UYG_UN_EE_O:
				case  UYG_UN_EE_A:
					skip=false;
					uly.Append('е');//е*/
					break;
				case  UYG_UN_I_6:
				case  UYG_UN_I_Y:
				case  UYG_UN_I_B:
				case  UYG_UN_I_O:
				case  UYG_UN_I_A:
					skip=false;
					uly.Append('и');//и*/
					break;
				case  UYG_UN_Y_6:
				case  UYG_UN_Y_Y:
				case  UYG_UN_Y_B:
				case  UYG_UN_Y_O:
				case  UYG_UN_Y_A:
					skip=false;
					uly.Append('й');//й*/
					break;
				default:
					skip=true;
					uly.Append(U2LSBelge(Herp+""));
					break;
			}
		}
		return uly.ToString();
	}



	static String U2LSBelge(string Herp) {
		String ret;
		if("؟"==Herp)
		{
			ret="?";
		}
		else if("،"==Herp)
		{
			ret=",";
		}
		else if("؛"==Herp)
		{
			ret=";";
		}
		else if("٭"==Herp)
		{
			ret="*";
		}
		else if("“"==Herp || "„"==Herp || "&#8220;"==Herp || "&#8222;"==Herp)
		{
			ret="«";
		}
		else if("”"==Herp || "‟"==Herp|| "&#8221;"==Herp || "&#8223;"==Herp)
		{
			ret="»";
		}
		else {
			ret=Herp;
		}
		return ret;
	}
	
	
	
	public static string FromUighursoft(string text){
		StringBuilder newText=new StringBuilder();
		char ret;
		foreach(char Herp in text){
			switch(Herp)
			{
				case  '\u0629':
					ret=UYG_UN_E_6 ;
					break;
				case  '\u0635':
					ret=UYG_UN_P_6;
					break;
				case  '\u062d':
					ret=UYG_UN_CH_6;
					break;
				case  '\u0622':
					ret=UYG_UN_ZZ_6;
					break;
				case  '\u0636':
					ret=UYG_UN_G_6 ;
					break;
				case  '\u062B':
					ret=UYG_UN_NG_6 ;
					break;
				case  '\u0647':
					ret=UYG_UN_H_6 ;
					break;
				case  '\u0630':
					ret=UYG_UN_U_6 ;
					break;
				case  '\u0623':
					ret=UYG_UN_OO_6;
					break;
				case  '\u0649':
					ret=UYG_UN_UU_6;
					break;
				case  '\u0624':
					ret=UYG_UN_W_6;
					break;
				case  '\u0626':
					ret=UYG_UN_EE_6;
					break;
				case '\u06CC':
				case  '\u0639':
					ret=UYG_UN_I_6 ;
					break;
				case  '\u0638':
					ret=UYG_UN_HM_6;
					break;
				default:
					ret = Herp;
					break;
			}
			newText.Append(ret);
		}
		return newText.ToString();
	}
	
	public static string FromDuldul(string text){
		StringBuilder newText=new StringBuilder();
		char ret;
		foreach(char Herp in text){
			switch(Herp)
			{
				case  '\u0629':
					ret=UYG_UN_E_6 ;
					break;
				case  '\u062B':
					ret=UYG_UN_NG_6 ;
					break;
				case  '\u0647':
					ret=UYG_UN_H_6 ;
					break;
				case  '\u0630':
					ret=UYG_UN_U_6 ;
					break;
				case  '\u0623':
					ret=UYG_UN_OO_6;
					break;
				case  '\u0625':
					ret=UYG_UN_UU_6;
					break;
				case  '\u0624':
					ret=UYG_UN_W_6;
					break;
				case  '\u0635':
					ret=UYG_UN_EE_6;
					break;
				case  '\u0626':
					ret=UYG_UN_I_6;
					break;
				case  '\u0638':
					ret=UYG_UN_HM_6;
					break;
				default:
					ret = Herp;
					break;
			}
			newText.Append(ret);
		}
		return newText.ToString();
	}
	
	public static string FromBashqilar(string text){
		StringBuilder newText=new StringBuilder();
		char ret;
		foreach(char Herp in text){
			switch(Herp)
			{
				case '\u0647':
					ret = UYG_UN_E_6;
					break;
				case '\u06CC':      // towendiki ikkisi Izdinishta ishlitilgen Ikki herpning kodi
					ret=UYG_UN_I_6;
					break;
				case '\u0629':      // towendiki ikkisi Izdinishta ishlitilgen Ikki herpning kodi
					ret=UYG_UN_E_6;
					break;
				case '\u0635':                         //Bu www.uyghurmuqam.com ning bash betide uchiridi
					ret=UYG_UN_I_6;
					break;
				case  UYGE_UN_HA_Y:
				case  UYGE_UN_HA_A:
					newText.Append(UYG_UN_HM_6);
					ret=UYG_UN_A_6;
					break;
				case  UYGE_UN_HE_Y:
				case  UYGE_UN_HE_A:
					newText.Append(UYG_UN_HM_6);
					ret=UYG_UN_E_6;
					break;

				case  UYGE_UN_HO_Y:
				case  UYGE_UN_HO_A:
					newText.Append(UYG_UN_HM_6);
					ret=UYG_UN_O_6;
					break;
				case  UYGE_UN_HU_Y:
				case  UYGE_UN_HU_A:
					newText.Append(UYG_UN_HM_6);
					ret=UYG_UN_U_6;
					break;
				case  UYGE_UN_HOO_Y:
				case  UYGE_UN_HOO_A:
					newText.Append(UYG_UN_HM_6);
					ret=UYG_UN_OO_6;
					break;
				case  UYGE_UN_HUU_Y:
				case  UYGE_UN_HUU_A:
					newText.Append(UYG_UN_HM_6);
					ret=UYG_UN_UU_6;
					break;

				case  UYGE_UN_HEE_Y:
				case  UYGE_UN_HEE_A:
				case  UYGE_UN_HEE_B:
					newText.Append(UYG_UN_HM_6);
					ret=UYG_UN_EE_6;
					break;
				case  UYGE_UN_HI_Y:
				case  UYGE_UN_HI_A:
				case  UYGE_UN_HI_B:
					newText.Append(UYG_UN_HM_6);
					ret=UYG_UN_I_6;
					break;
				case UYG_UN_LA_6:
					newText.Append(UYG_UN_L_6);
					ret=UYG_UN_A_6;
					break;
				default:
					ret = Herp;
					break;
			}
			newText.Append(ret);
		}
		return newText.ToString();
	}
	
	public static YEZIQ Detect(string eslitxt){
		YEZIQ ret = YEZIQ.YOQ;
		UInt32 cntUEY = 0;
		UInt32 cntULY = 0;
		UInt32 cntUSY = 0;
		if(eslitxt.Length>5000){
			eslitxt = eslitxt.Substring(0,5000);
		}
		eslitxt = eslitxt.Replace(Uyghur.Sozghuch,"");
		foreach(char Herp in eslitxt)
		{
			if((Herp>=0x0041&&Herp<=0x005A) || (Herp>=0x0060 && Herp<=0x007a))
			{
				cntULY++;
			}
			else if(Herp>=0x0600 && Herp<0x067F)
			{
				cntUEY++;
			}
			else if(Herp>=0x0400 && Herp<0x047F)
			{
				cntUSY++;
			}
		}

		if(cntUEY>cntULY && cntUEY>cntUSY)
		{
			ret = YEZIQ.UEY;
		}
		else if(cntULY>cntUEY && cntULY>cntUSY)
		{
			ret = YEZIQ.ULY;
		}
		else if(cntUSY>cntUEY && cntUSY>cntULY)
		{
			ret = YEZIQ.USY;
		}
		return ret;
	}
	
	public static string USY2UEY(string esliBuf)
	{
		bool alUEY=false;
		StringBuilder newBuf = new StringBuilder();
		foreach(char Herp in esliBuf)
		{
			switch(Herp)
			{
				case '?':
					newBuf.Append('؟');
					alUEY=false;
					break;
				case ',':
					newBuf.Append('،');
					alUEY=false;
					break;
				case ';':
					newBuf.Append('؛');
					alUEY=false;
					break;
					
				case '\'':
				case '\u2019':
				case 'ъ':
					newBuf.Append(UYG_UN_HM_6);
					alUEY=true;
					break;

				case '\u0430':	 //а
				case '\u0410':  //А
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_A_6);
					alUEY=true;
					break;
				case  '\u04d9':	//ә*/
				case  '\u04d8':   //Ә
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_E_6);
					alUEY=true;
					break;
				case  '\u0435':	//е
				case  '\u0415':   //Е
				case  '\u044d':   //э
				case  '\u042d':   //Э
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_EE_6);
					alUEY=true;
					break;
				case  '\u0438':	//и
				case  '\u0418':   //И
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_I_6);
					alUEY=true;
					break;
				case  '\u043e':	//о
				case  '\u041e':   //О
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_O_6);
					alUEY=true;
					break;
				case  '\u0443':	//у
				case  '\u0423':   //У
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_U_6);
					alUEY=true;
					break;
				case  '\u04e9':	//ө
				case  '\u04e8':   //Ө
				case  '\u045e':   //ў
				case  '\u040e':   //Ў
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_OO_6);
					alUEY=true;
					break;
				case  '\u04af':	//ү
				case  '\u04AE':   //Ү
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_UU_6);
					alUEY=true;
					break;
				case  '\u0431':	//б
				case  '\u0411': //Б
					newBuf.Append(UYG_UN_B_6);
					alUEY=true;
					break;
				case  '\u043f':	//п
				case  '\u041F':  //П
					newBuf.Append(UYG_UN_P_6);
					alUEY=true;
					break;
				case  '\u0442':	//т
				case  '\u0422': //Т
					newBuf.Append(UYG_UN_T_6);
					alUEY=true;
					break;
				case  '\u0497':	//җ
				case  '\u0496':   //Җ
					newBuf.Append(UYG_UN_J_6);
					alUEY=true;
					break;
				case  '\u0447':	//ч
				case  '\u0427':  //Ч
					newBuf.Append(UYG_UN_CH_6);
					alUEY=true;
					break;
				case  '\u0445':	//х
				case  '\u0425':   //Х
					newBuf.Append(UYG_UN_X_6);
					alUEY=true;
					break;
				case  '\u0434':	//д
				case  '\u0414':   //Д
					newBuf.Append(UYG_UN_D_6);
					alUEY=true;
					break;
				case  '\u0440':	//р
				case  '\u0420':   //Р
					newBuf.Append(UYG_UN_R_6);
					alUEY=true;
					break;
				case  '\u0437':	//з
				case  '\u0417':   //З
					newBuf.Append(UYG_UN_Z_6);
					alUEY=true;
					break;
				case  '\u0436':	//ж
				case  '\u0416':   //Ж
					newBuf.Append(UYG_UN_ZZ_6);
					alUEY=true;
					break;
				case  '\u0441':	//с
				case  '\u0421':   //С
				case  '\u0426':   //Ц
				case  '\u0446':   //ц
					newBuf.Append(UYG_UN_S_6);
					alUEY=true;
					break;
				case  '\u0448':	//ш
				case  '\u0428':   //Ш
					newBuf.Append(UYG_UN_SH_6);
					alUEY=true;
					break;
				case  '\u0493':	//ғ
				case  '\u0492':   //Ғ
					newBuf.Append(UYG_UN_GH_6);
					alUEY=true;
					break;
				case  '\u0444':	//ф
				case  '\u0424':   //Ф
					newBuf.Append(UYG_UN_F_6);
					alUEY=true;
					break;
				case  '\u049b':	//қ
				case  '\u049A':   //Қ
					newBuf.Append(UYG_UN_Q_6);
					alUEY=true;
					break;
				case  '\u043a':	//к
				case  '\u041a':   //К
					newBuf.Append(UYG_UN_K_6);
					alUEY=true;
					break;
				case  '\u0433':	//г
				case  '\u0413':   //Г
					newBuf.Append(UYG_UN_G_6);
					alUEY=true;
					break;
				case  '\u04a3':	//ң
				case  '\u04a2':   //Ң
					newBuf.Append(UYG_UN_NG_6);
					alUEY=true;
					break;
				case  '\u043b':	//л
				case  '\u041B':   //Л
					newBuf.Append(UYG_UN_L_6);
					alUEY=true;
					break;
				case  '\u043c':	//м
				case  '\u041c':   //М
					newBuf.Append(UYG_UN_M_6);
					alUEY=true;
					break;
				case  '\u043d':	//н
				case  '\u041D':   //Н
					newBuf.Append(UYG_UN_N_6);
					alUEY=true;
					break;
				case  '\u04bb':	  //һ
				case  '\u04ba':   //Һ
				case  '\u04b3':   //ҳ
				case  '\u04b2':   //ҳ
					newBuf.Append(UYG_UN_H_6);
					alUEY=true;
					break;
				case  '\u0432':	//в
				case  '\u0412':   //В
					newBuf.Append(UYG_UN_W_6);
					alUEY=true;
					break;
				case  '\u0439':	  //й
				case  '\u0419':   //Й
					newBuf.Append(UYG_UN_Y_6);
					alUEY=true;
					break;
				case  '\u044f':   //я
				case  '\u042F':   //Я
					newBuf.Append(UYG_UN_Y_6);
					newBuf.Append(UYG_UN_A_6);
					alUEY=true;
					break;
				case  '\u044E': //ю
				case  '\u042E': //Ю
					newBuf.Append(UYG_UN_Y_6);
					newBuf.Append(UYG_UN_U_6);
					alUEY=true;
					break;
				case '\u0451': //
				case '\u0401': //
					newBuf.Append(UYG_UN_Y_6);
					newBuf.Append(UYG_UN_O_6);
					alUEY=true;
					break;
				case '\u0406': //І
				case '\u0456': //і
					newBuf.Append(UYG_UN_I_6);
					alUEY=true;
					break;
				case '\u04b0': //Ұ
				case '\u04b1': //ұ
					newBuf.Append(UYG_UN_U_6);
					alUEY=true;
					break;
				case '\u042b': //Ы
				case '\u044b': //ы
					newBuf.Append(UYG_UN_I_6);
					alUEY=true;
					break;
				case 'ь':
					break;
				default:
					alUEY=false;
					newBuf.Append(Herp);
					break;
			}
			//if(Herp=='\u044A' || Herp=='\u044C' || Herp== '\u042C')
			//{
			//	alUEY=false;
			//}
		}
		return newBuf.ToString();
	}
	

	public static string ULY2UEY(string esliBuf)
	{
		bool alUEY=false;
		StringBuilder newBuf = new StringBuilder();
		String newesliBuf = esliBuf.Replace("Gh",UYG_UN_GH_6+"").Replace("gh",UYG_UN_GH_6+"").Replace("GH",UYG_UN_GH_6+"").Replace("gH",UYG_UN_GH_6+"")
			.Replace("ng",UYG_UN_NG_6+"").Replace("NG",UYG_UN_NG_6+"").Replace("Ng",UYG_UN_NG_6+"").Replace("nG",UYG_UN_NG_6+"")
			.Replace("Sh",UYG_UN_SH_6+"").Replace("sh",UYG_UN_SH_6+"").Replace("sH",UYG_UN_SH_6+"").Replace("SH",UYG_UN_SH_6+"")
			.Replace("Ch",UYG_UN_CH_6+"").Replace("ch",UYG_UN_CH_6+"").Replace("cH",UYG_UN_CH_6+"").Replace("CH",UYG_UN_CH_6+"")
			.Replace("Zh",UYG_UN_ZZ_6+"").Replace("zh",UYG_UN_ZZ_6+"").Replace("zH",UYG_UN_ZZ_6+"").Replace("ZH",UYG_UN_ZZ_6+"")
			.Replace("<<","«").Replace(">>","»");

		char Herp;
		for(int i=0;i<newesliBuf.Length;i++)
		{
			Herp = newesliBuf[i];
			switch(Herp)
			{
				case '?':
					newBuf.Append('؟');
					alUEY=false;
					break;
				case ',':
					newBuf.Append('،');
					alUEY=false;
					break;
				case ';':
					newBuf.Append('؛');
					alUEY=false;
					break;
				case '\'':
				case '\u2019':
					if((i+1)<newesliBuf.Length &&
					   newesliBuf[i+1] == 'A'||newesliBuf[i+1] == 'a'||
					   newesliBuf[i+1] == 'E'||newesliBuf[i+1] == 'e'||
					   newesliBuf[i+1] == 'I'||newesliBuf[i+1] == 'i'||
					   newesliBuf[i+1] == 'O'||newesliBuf[i+1] == 'o'||
					   newesliBuf[i+1] == 'U'||newesliBuf[i+1] == 'u'||
					   newesliBuf[i+1] == 'É'||newesliBuf[i+1] == 'é'||
					   newesliBuf[i+1] == 'Ë'||newesliBuf[i+1] == 'ë'||
					   newesliBuf[i+1] == 'Ö'||newesliBuf[i+1] == 'ö'||
					   newesliBuf[i+1] == 'Ü'||newesliBuf[i+1] == 'ü'
					  )
					{
						newBuf.Append(UYG_UN_HM_6);
						alUEY=true;
					}
					break;
				case 'a':
				case 'A':
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_A_6);
					alUEY=false;
					break;
				case  'e':
				case  'E':
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_E_6);
					alUEY=false;
					break;
				case  'é':
				case  'É':
				case  'ë':
				case  'Ë':
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_EE_6);
					alUEY=false;
					break;
				case  'i':
				case  'I':
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_I_6);
					alUEY=false;
					break;
				case  'o':
				case  'O':
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_O_6);
					alUEY=false;
					break;
				case  'u':
				case  'U':
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_U_6);
					alUEY=false;
					break;
				case  'ö':
				case  'Ö':
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_OO_6);
					alUEY=false;
					break;
				case  'ü':
				case  'Ü':
					if(alUEY==false)
					{
						newBuf.Append(UYG_UN_HM_6);
					}
					newBuf.Append(UYG_UN_UU_6);
					alUEY=false;
					break;
				case  'b':
				case  'B':
					newBuf.Append(UYG_UN_B_6);
					alUEY=true;
					break;
				case  'p':
				case  'P':
					newBuf.Append(UYG_UN_P_6);
					alUEY=true;
					break;
				case  't':
				case  'T':
					newBuf.Append(UYG_UN_T_6);
					alUEY=true;
					break;
				case  'j':
				case  'J':
					newBuf.Append(UYG_UN_J_6);
					alUEY=true;
					break;
				case  'x':
				case  'X':
					newBuf.Append(UYG_UN_X_6);
					alUEY=true;
					break;
				case  'd':
				case  'D':
					newBuf.Append(UYG_UN_D_6);
					alUEY=true;
					break;
				case  'r':
				case  'R':
					newBuf.Append(UYG_UN_R_6);
					alUEY=true;
					break;
				case  'z':
				case  'Z':
					newBuf.Append(UYG_UN_Z_6);
					alUEY=true;
					break;
				case  's':
				case  'S':
					newBuf.Append(UYG_UN_S_6);
					alUEY=true;
					break;
				case  'f':
				case  'F':
					newBuf.Append(UYG_UN_F_6);
					alUEY=true;
					break;
				case  'q':
				case  'Q':
					newBuf.Append(UYG_UN_Q_6);
					alUEY=true;
					break;
				case  'k':
				case  'K':
					newBuf.Append(UYG_UN_K_6);
					alUEY=true;
					break;
				case  'g':
				case  'G':
					newBuf.Append(UYG_UN_G_6);
					alUEY=true;
					break;
				case  'l':
				case  'L':
					newBuf.Append(UYG_UN_L_6);
					alUEY=true;
					break;
				case  'm':
				case  'M':
					newBuf.Append(UYG_UN_M_6);
					alUEY=true;
					break;
				case  'n':
				case  'N':
					newBuf.Append(UYG_UN_N_6);
					alUEY=true;
					break;
				case  'h':
				case  'H':
					newBuf.Append(UYG_UN_H_6);
					alUEY=true;
					break;
				case  'w':
				case  'W':
					newBuf.Append(UYG_UN_W_6);
					alUEY=true;
					break;
				case  'y':
				case  'Y':
					newBuf.Append(UYG_UN_Y_6);
					alUEY=true;
					break;
				case UYG_UN_GH_6:
				case UYG_UN_CH_6:
				case UYG_UN_SH_6:
				case UYG_UN_ZZ_6:
				case UYG_UN_NG_6:
					newBuf.Append(Herp);
					alUEY=true;
					break;
				default:
					alUEY=false;
					newBuf.Append(Herp);
					break;
			}
		}
		return newBuf.ToString();
	}

	
	public static string ToUEY(string esliTxt){
		YEZIQ ret = Detect(esliTxt);
		if(ret == YEZIQ.ULY){
			return ULY2UEY(esliTxt);
		}
		else if(ret == YEZIQ.USY){
			return USY2UEY(esliTxt);
		}
		else{
			return null;
		}
	}


	
	public static string ToULY(string esliTxt){
		YEZIQ ret = Detect(esliTxt);
		if(ret == YEZIQ.UEY){
			return UEY2ULY(esliTxt);
		}
		else if(ret == YEZIQ.USY){
			return　USY2ULY(esliTxt);
		}
		else{
			return null;
		}
	}
	
	public static string ToUSY(string esliTxt){
		YEZIQ ret = Detect(esliTxt);
		if(ret == YEZIQ.UEY){
			return UEY2USY(esliTxt);
		}
		else if(ret == YEZIQ.ULY){
			return ULY2USY(esliTxt);
		}
		else{
			return null;
		}
	}
	
	
	public static string ULY2USY(string esliBuf)
	{
		StringBuilder newBuf = new StringBuilder();
		String newesliBuf = esliBuf.Replace("GH","Ғ").Replace("Gh","Ғ").Replace("gh","ғ").Replace("gH","ғ")
			.Replace("NG","Ң").Replace("Ng","Ң").Replace("ng","ң").Replace("nG","ң")
			.Replace("SH","Ш").Replace("Sh","Ш").Replace("sh","ш").Replace("sH","ш")
			.Replace("CH","Ч").Replace("Ch","Ч").Replace("ch","ч").Replace("cH","ч")
			.Replace("ZH","Ж").Replace("Zh","Ж").Replace("zh","ж").Replace("zH","ж");
		
		bool aldiN = false;
		foreach(char Herp in newesliBuf)
		{
			switch(Herp)
			{
				case 'A':
					aldiN = false;
					newBuf.Append('А');
					break;
				case 'a':
					aldiN = false;
					newBuf.Append('а');
					break;
				case 'B':
					aldiN = false;
					newBuf.Append('Б');
					break;
				case 'b':
					aldiN = false;
					newBuf.Append('б');
					break;
				case 'D':
					aldiN = false;
					newBuf.Append('Д');
					break;
				case 'd':
					aldiN = false;
					newBuf.Append('д');
					break;
				case 'É':
					aldiN = false;
					newBuf.Append('Е');
					break;
				case 'é':
					aldiN = false;
					newBuf.Append('е');
					break;
				case 'E':
					aldiN = false;
					newBuf.Append('Ә');
					break;
				case 'e':
					aldiN = false;
					newBuf.Append('ә');
					break;
				case 'F':
					aldiN = false;
					newBuf.Append('Ф');
					break;
				case 'f':
					aldiN = false;
					newBuf.Append('ф');
					break;
				case 'G':
					aldiN = false;
					newBuf.Append('Г');
					break;
				case 'g':
					aldiN = false;
					newBuf.Append('г');
					break;
				case 'H':
					aldiN = false;;
					newBuf.Append('Һ');
					break;
				case 'h':
					aldiN = false;
					newBuf.Append('һ');
					break;
				case 'I':
					aldiN = false;
					newBuf.Append('И');
					break;
				case 'i':
					aldiN = false;
					newBuf.Append('и');
					break;
				case 'J':
					aldiN = false;
					newBuf.Append('Җ');
					break;
				case 'j':
					aldiN = false;
					newBuf.Append('җ');
					break;
				case 'K':
					aldiN = false;
					newBuf.Append('К');
					break;
				case 'k':
					aldiN = false;
					newBuf.Append('к');
					break;
				case 'L':
					aldiN = false;
					newBuf.Append('Л');
					break;
				case 'l':
					aldiN = false;
					newBuf.Append('л');
					break;
				case 'M':
					aldiN = false;
					newBuf.Append('М');
					break;
				case 'm':
					aldiN = false;
					newBuf.Append('м');
					break;
				case 'N':
					aldiN = true;
					newBuf.Append('Н');
					break;
				case 'n':
					aldiN = true;
					newBuf.Append('н');
					break;
				case 'Ö':
					aldiN = false;
					newBuf.Append('Ө');
					break;
				case 'ö':
					aldiN = false;
					newBuf.Append('ө');
					break;
				case 'O':
					aldiN = false;
					newBuf.Append('О');
					break;
				case 'o':
					aldiN = false;
					newBuf.Append('о');
					break;
				case 'P':
					aldiN = false;
					newBuf.Append('П');
					break;
				case 'p':
					aldiN = false;
					newBuf.Append('п');
					break;
				case 'Q':
					aldiN = false;
					newBuf.Append('Қ');
					break;
				case 'q':
					aldiN = false;
					newBuf.Append('қ');
					break;
				case 'R':
					aldiN = false;
					newBuf.Append('Р');
					break;
				case 'r':
					aldiN = false;
					newBuf.Append('р');
					break;
				case 'S':
					aldiN = false;
					newBuf.Append('С');
					break;

				case 's':
					aldiN = false;
					newBuf.Append('с');
					break;
				case 'T':
					aldiN = false;
					newBuf.Append('Т');
					break;
				case 't':
					aldiN = false;
					newBuf.Append('т');
					break;
				case 'Ü':
					aldiN = false;
					newBuf.Append('Ү');
					break;
				case 'ü':
					aldiN = false;
					newBuf.Append('ү');
					break;
				case 'U':
					aldiN = false;
					newBuf.Append('У');
					break;
				case 'u':
					aldiN = false;
					newBuf.Append('у');
					break;
				case 'W':
					aldiN = false;
					newBuf.Append('В');
					break;
				case 'w':
					aldiN = false;
					newBuf.Append('в');
					break;
				case 'X':
					aldiN = false;
					newBuf.Append('Х');
					break;
				case 'x':
					aldiN = false;
					newBuf.Append('х');
					break;
				case 'Y':
					aldiN = false;
					newBuf.Append('Й');
					break;
				case 'y':
					aldiN = false;
					newBuf.Append('й');
					break;
				case 'Z':
					aldiN = false;
					newBuf.Append('З');
					break;
				case 'z':
					aldiN = false;
					newBuf.Append('з');
					break;
				case 'C':
					aldiN = false;
					newBuf.Append('С');
					break;
				case 'c':
					aldiN = false;
					newBuf.Append('с');
					break;
				case '’':
					if(aldiN){
						
					}
					else{
						newBuf.Append('ъ');
					}
					aldiN = false;
					break;
				default:
					aldiN = false;
					newBuf.Append(Herp);
					break;
			}
		}
		return newBuf.ToString();
	}
	
	
	public static string USY2ULY(string esliBuf)
	{
		StringBuilder newBuf = new StringBuilder();
		foreach(char Herp in esliBuf)
		{
			switch(Herp)
			{
				case 'А':
					newBuf.Append('A');
					break;
				case 'а':
					newBuf.Append('a');
					break;
				case 'Б':
					newBuf.Append('B');
					break;
				case 'б':
					newBuf.Append('b');
					break;
				case 'Д':
					newBuf.Append('D');
					break;
				case 'д':
					newBuf.Append('d');
					break;
				case 'Е':
					newBuf.Append('É');
					break;
				case 'е':
					newBuf.Append('é');
					break;
				case 'Ә':
					newBuf.Append('E');
					break;
				case 'ә':
					newBuf.Append('e');
					break;
				case 'Ф':
					newBuf.Append('F');
					break;
				case 'ф':
					newBuf.Append('f');
					break;
				case 'Г':
					newBuf.Append('G');
					break;
				case 'г':
					newBuf.Append('g');
					break;
				case 'Һ':
					newBuf.Append('H');
					break;
				case 'һ':
					newBuf.Append('h');
					break;
				case 'И':
					newBuf.Append('I');
					break;
				case 'и':
					newBuf.Append('i');
					break;
				case 'Җ':
					newBuf.Append('J');
					break;
				case 'җ':
					newBuf.Append('j');
					break;
				case 'К':
					newBuf.Append('K');
					break;
				case 'к':
					newBuf.Append('k');
					break;
				case 'Л':
					newBuf.Append('L');
					break;
				case 'л':
					newBuf.Append('l');
					break;
				case 'М':
					newBuf.Append('M');
					break;
				case 'м':
					newBuf.Append('m');
					break;
				case 'Н':
					newBuf.Append('N');
					break;
				case 'н':
					newBuf.Append('n');
					break;
				case 'Ө':
					newBuf.Append('Ö');
					break;
				case 'ө':
					newBuf.Append('ö');
					break;
				case 'О':
					newBuf.Append('O');
					break;
				case 'о':
					newBuf.Append('o');
					break;
				case 'П':
					newBuf.Append('P');
					break;
				case 'п':
					newBuf.Append('p');
					break;
				case 'Қ':
					newBuf.Append('Q');
					break;
				case 'қ':
					newBuf.Append('q');
					break;
				case 'Р':
					newBuf.Append('R');
					break;
				case 'р':
					newBuf.Append('r');
					break;
				case 'С':
					newBuf.Append('S');
					break;
				case 'с':
					newBuf.Append('s');
					break;
				case 'Т':
					newBuf.Append('T');
					break;
				case 'т':
					newBuf.Append('t');
					break;
				case 'Ү':
					newBuf.Append('Ü');
					break;
				case 'ү':
					newBuf.Append('ü');
					break;
				case 'У':
					newBuf.Append('U');
					break;
				case 'у':
					newBuf.Append('u');
					break;
				case 'В':
					newBuf.Append('W');
					break;
				case 'в':
					newBuf.Append('w');
					break;
				case 'Х':
					newBuf.Append('X');
					break;
				case 'х':
					newBuf.Append('x');
					break;
				case 'Й':
					newBuf.Append('Y');
					break;
				case 'й':
					newBuf.Append('y');
					break;
				case 'З':
					newBuf.Append('Z');
					break;
				case 'з':
					newBuf.Append('z');
					break;
					
				case 'Ғ':
					newBuf.Append('G');
					newBuf.Append('h');
					break;
				case 'ғ':
					newBuf.Append('g');
					newBuf.Append('h');
					break;
				case 'Ң':
					newBuf.Append('N');
					newBuf.Append('g');
					break;
				case 'ң':
					newBuf.Append('n');
					newBuf.Append('g');
					break;
				case 'Ш':
					newBuf.Append('S');
					newBuf.Append('h');
					break;
				case 'ш':
					newBuf.Append('s');
					newBuf.Append('h');
					break;
				case 'Ч':
					newBuf.Append('C');
					newBuf.Append('h');
					break;
				case 'ч':
					newBuf.Append('c');
					newBuf.Append('h');
					break;
				case 'Ж':
					newBuf.Append('Z');
					newBuf.Append('h');
					break;
				case 'ж':
					newBuf.Append('z');
					newBuf.Append('h');
					break;
					
				case 'Я':
					newBuf.Append('Y');
					newBuf.Append('a');
					break;
				case 'я':
					newBuf.Append('y');
					newBuf.Append('a');
					break;

				case 'Ю':
					newBuf.Append('Y');
					newBuf.Append('u');
					break;
				case 'ю':
					newBuf.Append('y');
					newBuf.Append('u');
					break;

				case 'Ё':
					newBuf.Append('Y');
					newBuf.Append('o');
					break;
				case 'ё':
					newBuf.Append('y');
					newBuf.Append('o');
					break;
				case 'ъ':
					newBuf.Append('’');
					break;
					
				case '\u042b': //Ы
				case '\u044b': //ы
					newBuf.Append('i');
					break;
				case 'ь':
					break;
				default:
					newBuf.Append(Herp);
					break;
			}
		}
		return newBuf.ToString();
	}
	
	
	public static string KeyToUEY(string key){
		string ueykey=key;
		switch (key){
			case "q":
			case "Q":
				ueykey = UYG_UN_CH_6 + "";
				break;
				
			case "w":
			case "W":
				ueykey = UYG_UN_W_6 + "";
				break;

			case "e":
			case "E":
				ueykey = UYG_UN_EE_6 + "";
				break;
				
			case "r":
			case "R":
				ueykey = UYG_UN_R_6 + "";
				break;
				
			case "t":
			case "T":
				ueykey = UYG_UN_T_6 + "";
				break;

			case "y":
			case "Y":
				ueykey = UYG_UN_Y_6 + "";
				break;

			case "u":
			case "U":
				ueykey = UYG_UN_U_6 + "";
				break;
			case "i":
			case "I":
				ueykey = UYG_UN_NG_6 + "";
				break;
				
			case "o":
			case "O":
				ueykey = UYG_UN_O_6 + "";
				break;
				
			case "p":
			case "P":
				ueykey = UYG_UN_P_6 + "";
				break;

			case "{":
				ueykey = UYG_UN_SOL_QOSH_TIRNAQ+"";
				break;
				
			case "a":
			case "A":
				ueykey = UYG_UN_H_6 + "";
				break;
				
			case "s":
			case "S":
				ueykey = UYG_UN_S_6 + "";
				break;

			case "d":
				ueykey = UYG_UN_D_6 + "";
				break;
			case "D":
				ueykey = UYG_UN_ZZ_6 + "";
				break;
			case "f":
				ueykey = UYG_UN_A_6 + "";
				break;
			case "F":
				ueykey = UYG_UN_F_6 + "";
				break;
				
			case "g":
				ueykey = UYG_UN_E_6 + "";
				break;
			case "G":
				ueykey = UYG_UN_G_6 + "";
				break;
				
			case "h":
				ueykey = UYG_UN_I_6 + "";
				break;
			case "H":
				ueykey = UYG_UN_X_6 + "";
				break;

			case "j":
				ueykey = UYG_UN_Q_6 + "";
				break;
			case "J":
				ueykey = UYG_UN_J_6 + "";
				break;
				
			case "k":
				ueykey = UYG_UN_K_6 + "";
				break;
			case "K":
				ueykey = UYG_UN_OO_6 + "";
				break;
				
			case "l":
			case "L":
				ueykey = UYG_UN_L_6 + "";
				break;
				
			case ";":
				ueykey = UYG_UN_CHEKITPESH + "";
				break;
				
			case "}":
				ueykey = UYG_UN_ONG_QOSH_TIRNAQ+"";
				break;

			case "z":
			case "Z":
				ueykey = UYG_UN_Z_6 + "";
				break;

			case "x":
			case "X":
				ueykey = UYG_UN_SH_6 + "";
				break;
				
			case "c":
			case "C":
				ueykey = UYG_UN_GH_6 + "";
				break;
				
			case "v":
			case "V":
				ueykey = UYG_UN_UU_6 + "";
				break;
				
			case "b":
			case "B":
				ueykey = UYG_UN_B_6 + "";
				break;
				
			case "n":
			case "N":
				ueykey = UYG_UN_N_6 + "";
				break;
			case "m":
			case "M":
				ueykey = UYG_UN_M_6 + "";
				break;
				
			case ",":
				ueykey = UYG_UN_PESH + "";
				break;

			case "<":
				ueykey = UYG_UN_KICHIK + "";
				break;

			case "/":
				ueykey = UYG_UN_HM_6 + "";
				break;
				
			case "?":
				ueykey = UYG_UN_SOAL + "";
				break;

			case ">":
				ueykey = UYG_UN_CHONG + "";
				break;

			case "\\":
				ueykey = UYG_UN_SIZIQ + "";
				break;
			case "|":
				ueykey = UYG_UN_KOPCHEKIT + "";
				break;
		}
		return ueykey;
	}
	
	public static string KeyToULY(string key){
		string ulykey=key;
		switch (key){
			case "<":
				ulykey = UYG_UN_KICHIK + "";
				break;
			case ">":
				ulykey = UYG_UN_CHONG + "";
				break;
			case "`":
				ulykey = "’";
				break;
			case "\\":
				ulykey = UYG_UN_SIZIQ + "";
				break;
			case "|":
				ulykey = UYG_UN_KOPCHEKIT + "";
				break;
			case "}":
				ulykey = UYG_UN_ONG_QOSH_TIRNAQ+"";
				break;
			case "{":
				ulykey = UYG_UN_SOL_QOSH_TIRNAQ+"";
				break;
		}
		return ulykey;
	}
	
	static char BGD_WEItoUnicode(char inWei)
	{
		char ret = inWei;
		switch (inWei) {
			case  '\uf9a1':
			case  '\ub0bf':
			case  '\ufaa1':
			case  '\ub1df':
				ret =  '\u0627';	//A
				break;
			case  '\uf9a2':
			case  '\ub0c0':
			case  '\ufaa2':
			case  '\ub1e0':
				ret =  '\u06d5';	//AA
				break;
			case  '\uf9af':
			case  '\ub1a1':
			case  '\ufaaf':
			case  '\ub1c1':
			case  '\ufab0':
			case  '\ub1e1':
			case  '\uf9b0':
			case  '\ub0c1':
				ret =  '\u0628';	//B  bash
				break;
			case  '\uf9b1':
			case  '\ub1a2':
			case  '\ufab1':
			case  '\ub1c2':
			case  '\ufab2':
			case  '\ub1e2':
			case  '\uf9b2':
			case  '\ub0c2':
				ret =  '\u067e';	//P
				break;
			case  '\uf9b3':
			case  '\ub1a3':
			case  '\ufab3':
			case  '\ub1c3':
			case  '\ufab4':
			case  '\ub1e3':
			case  '\uf9b4':
			case  '\ub0c3':
				ret =  '\u062a';	//T
				break;
			case  '\uf9b5':
			case  '\ub1a4':
			case  '\ufab5':
			case  '\ub1c4':
			case  '\ufab6':
			case  '\ub1e4':
			case  '\uf9b6':
			case  '\ub0c4':
				ret =  '\u062c';	//J
				break;
			case  '\uf9b7':
			case  '\ub1a5':
			case  '\ufab7':
			case  '\ub1c5':
			case  '\ufab8':
			case  '\ub1e5':
			case  '\uf9b8':
			case  '\ub0c5':
				ret =  '\u0686';	//Q
				break;
			case  '\uf9b9':
			case  '\ub1a6':
			case  '\ufab9':
			case  '\ub1c6':
			case  '\ufaba':
			case  '\ub1e6':
			case  '\uf9ba':
			case  '\ub0c6':
				ret =  '\u062e';	//H
				break;
			case  '\uf9e7':
			case  '\ub0c7':
			case  '\ufae7':
			case  '\ub1e7':
				ret =  '\u062f';	//D
				break;
			case  '\uf9e8':
			case  '\ub0c8':
			case  '\ufae8':
			case  '\ub1e8':
				ret =  '\u0631';	//R
				break;
			case  '\uf9e9':
			case  '\ub0c9':
			case  '\ufae9':
			case  '\ub1e9':
				ret =  '\u0632';	//Z
				break;
			case  '\uf9ea':
			case  '\ub0ca':
			case  '\ufaea':
			case  '\ub1ea':
				ret =  '\u0698';	//ZZ
				break;
			case  '\uf9bb':
			case  '\ub1ab':
			case  '\ufabb':
			case  '\ub1cb':
			case  '\ufabc':
			case  '\ub1eb':
			case  '\uf9bc':
			case  '\ub0cb':
				ret =  '\u0633';	//S
				break;
			case  '\uf9bd':
			case  '\ub1ac':
			case  '\ufabd':
			case  '\ub1cc':
			case  '\ufabe':
			case  '\ub1ec':
			case  '\uf9be':
			case  '\ub0cc':
				ret =  '\u0634';	//X
				break;
			case  '\uf9bf':
			case  '\ub1ad':
			case  '\ufabf':
			case  '\ub1cd':
			case  '\ufac0':
			case  '\ub1ed':
			case  '\uf9c0':
			case  '\ub0cd':
				ret =  '\u063a';	//GH
				break;
			case  '\uf9c1':
			case  '\ub1ae':
			case  '\ufac1':
			case  '\ub1ce':
			case  '\ufac2':
			case  '\ub1ee':
			case  '\uf9c2':
			case  '\ub0ce':
				ret =  '\u0641';	//F
				break;
			case  '\uf9c3':
			case  '\ub1af':
			case  '\ufac3':
			case  '\ub1cf':
			case  '\ufac4':
			case  '\ub1ef':
			case  '\uf9c4':
			case  '\ub0cf':
				ret =  '\u0642';	//KK
				break;
			case  '\uf9c5':
			case  '\ub1b0':
			case  '\ufac5':
			case  '\ub1d0':
			case  '\ufac6':
			case  '\ub1f0':
			case  '\uf9c6':
			case  '\ub0d0':
				ret =  '\u0643';	//K
				break;
			case  '\uf9c7':
			case  '\ub1b1':
			case  '\ufac7':
			case  '\ub1d1':
			case  '\ufac8':
			case  '\ub1f1':
			case  '\uf9c8':
			case  '\ub0d1':
				ret =  '\u06af';	//G
				break;
			case  '\uf9c9':
			case  '\ub1b2':
			case  '\ufac9':
			case  '\ub1d2':
			case  '\ufaca':
			case  '\ub1f2':
			case  '\uf9ca':
			case  '\ub0d2':
				ret =  '\u06ad';	//NG
				break;
			case  '\uf9cb':
			case  '\ub1b3':
			case  '\ufacb':
			case  '\ub1d3':
			case  '\ufacc':
			case  '\ub1f3':
			case  '\uf9cc':
			case  '\ub0d3':
				ret =  '\u0644';	//L
				break;
			case  '\uf9cd':
			case  '\ub1b4':
			case  '\ufacd':
			case  '\ub1d4':
			case  '\uface':
			case  '\ub1f4':
			case  '\uf9ce':
			case  '\ub0d4':
				ret =  '\u0645';	//M
				break;
			case  '\uf9cf':
			case  '\ub1b5':
			case  '\ufacf':
			case  '\ub1d5':
			case  '\ufad0':
			case  '\ub1f5':
			case  '\uf9d0':
			case  '\ub0d5':
				ret =  '\u0646';	//N
				break;
			case  '\uf9e1':
			case  '\ub1b6':
			case  '\ufae1':
			case  '\ub1d6':
			case  '\ufae2':
			case  '\ub1f6':
			case  '\uf9e2':
			case  '\ub0d6':
				ret =  '\u06be';	//HH
				break;
			case  '\uf9a3':
			case  '\ub0d7':
			case  '\ufaa3':
			case  '\ub1f7':
				ret =  '\u0648';	//O
				break;
			case  '\uf9a4':
			case  '\ub0d8':
			case  '\ufaa4':
			case  '\ub1f8':
				ret =  '\u06c7';	//U
				break;
			case  '\uf9a5':
			case  '\ub0d9':
			case  '\ufaa5':
			case  '\ub1f9':
				ret =  '\u06c6';	//OO
				break;
			case  '\uf9a6':
			case  '\ub0da':
			case  '\ufaa6':
			case  '\ub1fa':
				ret =  '\u06c8';	//UU
				break;
			case  '\uf9eb':
			case  '\ub0db':
			case  '\ufaeb':
			case  '\ub1fb':
				ret =  '\u06cb';	//W
				break;
			case  '\uf9ab':
			case  '\ub1bc':
			case  '\ufaab':
			case  '\ub1dc':
			case  '\ufaac':
			case  '\ub1fc':
			case  '\uf9ac':
			case  '\ub0dc':
				ret =  '\u06d0';	//E
				break;
			case  '\uf9ad':
			case  '\ub1bd':
			case  '\ufaad':
			case  '\ub1dd':
			case  '\ufaae':
			case  '\ub1fd':
			case  '\uf9ae':
			case  '\ub0dd':
				ret =  '\u0649';	//I
				break;
			case  '\uf9d1':
			case  '\ub1be':
			case  '\ufad1':
			case  '\ub1de':
			case  '\ufad2':
			case  '\ub1fe':
			case  '\uf9d2':
			case  '\ub0de':
				ret =  '\u064a';	//Y
				break;
			case  '\uf9df':
			case  '\uf9e0':
			case  '\ub0bc':
			case  '\ufadf':
			case  '\ub0fc':
			case  '\ufae0':
				ret =  '\u0626';	//amza  bash
				break;
			case  '\uf9ef':
			case  '\ub0aa':
				ret =  '\u002a';	//*
				break;
			case  '\uf9f0':
			case  '\ub0b0':
				ret =  '\u0030';	//0
				break;
			case  '\uf9f1':
			case  '\ub0b1':
				ret =  '\u0031';	//1
				break;
			case  '\uf9f2':
			case  '\ub0b2':
				ret =  '\u0032';	//2
				break;
			case  '\uf9f3':
			case  '\ub0b3':
				ret =  '\u0033';	//3
				break;
			case  '\uf9f4':
			case  '\ub0b4':
				ret =  '\u0034';	//4
				break;
			case  '\uf9f5':
			case  '\ub0b5':
				ret =  '\u0035';	//5
				break;
			case  '\uf9f6':
			case  '\ub0b6':
				ret =  '\u0036';	//6
				break;
			case  '\uf9f7':
			case  '\ub0b7':
				ret =  '\u0037';	//7
				break;
			case  '\uf9f8':
			case  '\ub0b8':
				ret =  '\u0038';	//8
				break;
			case  '\uf9f9':
			case  '\ub0b9':
				ret =  '\u0039';	//9
				break;
			case  '\uf9fa':
			case  '\ub0ab':
				ret =  '\u002b';	//+
				break;
			case  '\uf9fb':
			case  '\ub0eb':
				ret =  '\u002d';	//-
				break;
			case  '\uf9fc':
			case  '\ub1aa':
				ret =  '\u00f7';	//BULUSH
				break;
			case  '\uf9fd':
			case  '\ub1b8':
				ret =  '\u00d7';	//KOPAYTIX
				break;
			case  '\uf9fe':
			case  '\ub0bd':
				ret =  '\u003d';	//=
				break;
			case  '\ufaee':
			case  '\ub0ea':
				ret =  '\u2212';	//---
				break;
			case  '\ufaef':
			case  '\ub0ad':
				ret =  '\u002d';	//-
				break;
			case  '\ufaf0':
			case  '\ub0ac':
				ret =  '\u060c';	//
				break;
			case  '\ufaf1':
			case  '\ub0ae':
				ret =  '\u002e';	//.
				break;
			case  '\ufaf2':
			case  '\ub0bb':
				ret =  '\u061b';	//;
				break;
			case  '\ufaf3':
			case  '\ub0ba':
				ret =  '\u003a';	//':
				break;
			case  '\ufaf4':
			case  '\ub0f0':
				ret =  '\u002d';	//-
				break;
			case  '\ufaf5':
			case  '\ub0a1':
				ret =  '\u0021';	//!
				break;
			case  '\ufaf6':
			case  '\ub0af':
				ret =  '\u061f';	//?
				break;
			case  '\ufaf7':
			case  '\ub0a9':
				ret =  '\u0028';	//(
				break;
			case  '\ufaf8':
			case  '\ub0f8':
				ret =  '\u00ab';	//<<
				break;
			case  '\ufaf9':
			case  '\ub0a8':
				ret =  '\u0029';	//)
				break;
			case  '\ufafa':
			case  '\ub0f9':
				ret =  '\u00bb';	//>>
				break;
			case  '\ufafb':
			case  '\ub0f3':
				ret =  '\u003c';	//<
				break;
			case  '\ufafc':
			case  '\ub0f4':
				ret =  '\u201d';	//"
				break;
			case  '\ufafd':
			case  '\ub0f2':
				ret =  '\u003e';	//>
				break;
			case  '\ufafe':
			case  '\ub0f5':
				ret = '\u201c';
				break;
			case  '\ub1b7': // uzun siziq
				ret= '\u2015';
				break;
			case  '\ub0a5':  // %
				ret= '\u0025';
				break;
		}
		return ret;
	}

	public static string BoghdaToUnicode(byte[] Buffer)
	{
		return "";
	}

	public static string WeifangWinToUnicode(byte[] Buffer)
	{
		return "";
	}

	
	public static string WeifangDosToUnicode(byte[] Buffer)
	{
		Encoding gbEnc = Encoding.GetEncoding("gb2312");
		byte[] tmpBuff = new byte[2];
		StringBuilder strBuf    = new StringBuilder();
		int i = 0;
		char   Herp;
		char   nHerp;
		while(i<Buffer.Length)
		{
			tmpBuff[0]=Buffer[i++];
			if(tmpBuff[0]>=0xA1)
			{
				tmpBuff[1]=Buffer[i++];
				if(tmpBuff[1]>=0xA1)
				{
					Herp = (char)((tmpBuff[0]<<8)|(tmpBuff[1]));
					if((Herp>=0xb0a1 && Herp<0xb0ff) || (Herp>=0xb1a1 && Herp<0xb1ff))
					{
						if(Herp==0xb0fe || Herp==0xb0be)
						{
							strBuf.Append(UYG_UN_L_6);
							strBuf.Append(UYG_UN_A_6);
						}
						else
						{
							nHerp = BGD_WEItoUnicode(Herp);
							if(nHerp != Herp){
								strBuf.Append(nHerp);
							}
							else
							{
								strBuf.Append(gbEnc.GetChars(tmpBuff)[0]);
							}
						}
					}
					else
					{
						strBuf.Append(gbEnc.GetChars(tmpBuff)[0]);
					}
					
				}
			}
			else if(tmpBuff[0]<0x80)
			{
				Herp = (char)tmpBuff[0];
				strBuf.Append(Herp);
			}
		}
		
		//return strBuf.ToString();
		return ReverseNonUyg(strBuf.ToString());
	}
	
	static string ReverseNonUyg(string org)
	{
		Regex nonUyg = new Regex("[ a-zA-Z0-9]{2,}",RegexOptions.Compiled);
		string newStr = nonUyg.Replace(org,match=>
		                               {
		                               	char[] cc = match.Value.ToCharArray();
		                               	Array.Reverse(cc);
		                               	return new string(cc);
		                               });
		return newStr;
	}
}
