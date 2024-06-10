# AutoCodeGenerationVSIX V1.0
-TR
Katmanlı mimarileriniz de sürekli her katmana girip kod yazmaktan sıkıldıysanız bu paket tam size göre. Dotnet 8 versiyonunda inşa edildi. Kullanacağınız zaman boş solition içerisinde bir adet EntityLayer adında class library kurmanız gerekmekte. Ardından Models klasörüne oluşturacağınız sınıfı etkinleştirip sağ tık yaptığınız da Auto Code Generator butonuna basmanız yeterlidir. Diğer katmanlar kendiliğinden otomatik oluşup bütün class ve interfaceler otomatik olarak oluşturulacaktır. Zamanla vaktim olduğu müddetçe eklemeler ve güncellemeler yapmaya devam edeceğim. Sizde önerilerinizi veya eklenmesi iyi olur dediğiniz herhangi bir durumu bana iletebilirsiniz. 

Bu proje herkese açıktır. Dilediğiniz gibi indirip kendinize veya ihtiyaçlarınıza göre özelleştirip kendi custom projeleriniz de kullanabilirsiz.

Eklentimizde mevcut olan bilgiler altta yer almaktadır :
 -EntityLayer
   -Dtos
 -DataLayer
    -Context
    -Abstract
    -EntityFramework
    -Concrete
    -Repository
 -BusinessLayer
    -Abstract
    -ClientMessages
    -Concrete
 -Common
   -Helpers
   -Response
   
-EN
If you are tired of constantly writing code for each layer in your simple architectures, this package is perfect for you. Built on .NET 8, this extension requires you to first create a class library named EntityLayer in an empty solution. Then, simply activate the class you will create in the Models folder, right-click, and select the Auto Code Generator button. The other layers will be automatically created, and all classes and interfaces will be generated automatically. I will continue to make additions and updates as time allows. You can also share your suggestions or any situation you think would be good to add.

This project is open to everyone. Feel free to download, customize according to your needs, and use it in your custom projects.

The information available in our extension is listed below:
-EntityLayer
   -Dtos
 -DataLayer
    -Context
    -Abstract
    -EntityFramework
    -Concrete
    -Repository
 -BusinessLayer
    -Abstract
    -ClientMessages
    -Concrete
 -Common
   -Helpers
   -Response
