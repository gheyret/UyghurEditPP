# UyghurEdit++

Text Editor with Spell Check Ability for Uyghur
<p align="center">
  <img src="./Images/uyghur.png" width="200" height="200"/>
</p>

# UyghurEdit++
Mexsus Uyghurche tehrirlesh üchün tüzülgen, Imla tekshürüsh(Imlasi xata sözlerning astigha qizil siziq bilen dawamliq körsitip bérish), Yéziqlarni almashturush, OCR(Resimdiki tékistlerni tonush) iqtidari bolghan, heqsiz tehrirligüch.


Esli kodini chüshürüp özingiz yughurup(compile) qilip ishletsingiz bolidu. Eger teyyarsini ishletmekchi bolsingiz pestiki ulanmidin eng yéngisini chüshürüp ishliting.
Zip ni yéyipla ichidiki UyghurEditPP.exe ni ijra qilsingiz bolidu.

# Diqqet
Eger OCR da xataliq körülse, [ Visual Studio 2019 Runtime](https://support.microsoft.com/en-us/topic/the-latest-supported-visual-c-downloads-2647da03-1eea-4433-9aff-95f26a218cc0) ni ornitip sinap béqing. chünki Tesseract OCR bu ambargha béqinidiken.

<hr></hr>

### 0.6 neshri(2021/08/04)
   * Chapla ni ishletkende, eger Saqlash Taxtisi(ClipBoard) da resim bolsa, aptomatik halda OCR ni qozghitilidighan qilindi.
   
   **Chüshürüsh**: 
   [64 bitliq Windows üchün](https://github.com/gheyret/UyghurEditPP/releases/download/0.6/UyghurEditPP.zip),
   [32 bitliq Windows üchün](https://github.com/gheyret/UyghurEditPP/releases/download/0.6/UyghurEditPP32.zip)


### 0.5 neshri(2021/07/15)
   * Izdesh we almashturushtiki bezi xataliqlar tüzitildi.
   * OCR gha Türkche qoshuldi. Buning bilen Türkche yaki Türkche bilen Uyghurche arilash kelgen resimlik höjjetlerni tékistke aylandurushqa bolidu.
   
   **Chüshürüsh**: 
   [64 bitliq Windows üchün](https://github.com/gheyret/UyghurEditPP/releases/download/0.5/UyghurEditPP.zip),
   [32 bitliq Windows üchün](https://github.com/gheyret/UyghurEditPP/releases/download/0.5/UyghurEditPP32.zip)

### 0.4 neshri(2021/03/21)
   * Bir qisim xataliqlar tüzitildi. bolupmu, birer soz kirgüzüpla chashqinekning ong teripini bassa, axirliship kétidighan xataliq tüzitildi.
   * Latinche kirgüzüsh iqtidari qoshuldi(```<Ctrl>+<K>``` bassa, kunupka almishidu). Latinche kirgüzüsh halitide ö, ü, é lerni töwendiki usul arqiliq kirgüzgili bolidu.    
     * ```<Shift> + <o>``` -> ö         
     * ```<Shift> + <u>``` -> ü
     * ```<Shift> + <e>``` -> é 
     
     o,u,e,ö,ü,é larning chong yézilishini kirgüzüsh nur belgisining aldidiki herpni chong yézilishqa özgertidighan tézletme kunupka ```Ctl+U``` ni ishletsingiz bolidu.     
     
       
   **Chüshürüsh**: 
   [64 bitliq Windows üchün](https://github.com/gheyret/UyghurEditPP/releases/download/0.4/UyghurEditPP.zip),
   [32 bitliq Windows üchün](https://github.com/gheyret/UyghurEditPP/releases/download/0.4/UyghurEditPP32.zip)


### 0.3 neshri(2021/03/17)
   * OCR da Uyghurche, In’glizche, Xenzuche yéziqlarni tonush iqtidari qoshuldi. 
     buning bilen arilash yéziqlarni tonush emelge ashuruldi.
   * Bir qisim xataliqlar tüzitildi.
   * Bir qisim körsetme uchurlar tüzitildi.
       
   **Chüshürüsh**: 
   [64 bitliq Windows üchün](https://github.com/gheyret/UyghurEditPP/releases/download/0.3/UyghurEditPP.zip),
   [32 bitliq Windows üchün](https://github.com/gheyret/UyghurEditPP/releases/download/0.3/UyghurEditPP32.zip)

<hr></hr>

### 0.2 neshri(2021/03/05)
   * UyghurEdit++ ning tughi özgertildi.(layihilep bergen qérindishimizgha köp rehmet).
   * Izdesh we Almashturush iqtidari yaxshilandi.
   * Tehrirlewatqan höjjettin HTML yasash iqtidari qoshuldi.

   **Chüshürüsh**:
   [64 bitliq Windows üchün](https://github.com/gheyret/UyghurEditPP/releases/download/0.2/UyghurEditPP.zip) , [32 bitliq Windows üchün](https://github.com/gheyret/UyghurEditPP/releases/download/0.2/UyghurEditPP32.zip)

<hr></hr>

[Ishlitish qollanmisi](https://github.com/gheyret/UyghurEditPP/wiki/Addiy-Ishlitish-Qollanmisi)


# Ékran körünüshliri:
![](screenshot/uey.png)

![](screenshot/uly.png)

![](screenshot/usy.png)

![](screenshot/ocrnew.png)

# Ishlitilgen ochuq kodlar
Avalonedit: https://github.com/icsharpcode/AvalonEdit

DynaJson: https://github.com/fujieda/DynaJson

Tesseract .Net: https://github.com/charlesw/tesseract

SymSpell: https://github.com/wolfgarbe/SymSpell (Hazirche bu ishlitilmidi. emma esli kodi saqlinip qélindi)
