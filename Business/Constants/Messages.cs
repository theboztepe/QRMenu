namespace Business.Constants
{
    public static class Messages
    {
        #region User
        public static string UserRegistered = "Kayıt başarılı.";
        public static string UserNotFound = "Kullanıcı adı veya şifre hatalı.";
        public static string SuccessfulLogin = "Giriş başarılı.";
        public static string AccessTokenCreated = "Token oluşturuldu.";
        public static string UserAlreadyExists = "Kullanıcı zaten mevcut.";
        public static string UnSigned = "UnSigned";
        public static string UserClosed = "Hesap kapalı.";
        public static string QRNotFound = "İlgili QR kodu için menü bulunamadı.";
        #endregion

        #region Category
        public static string CategoryAdded = "Kategori eklendi.";
        public static string CategoryUpdated = "Kategori güncellendi.";
        public static string CategoryRemoved = "Kategori silindi.";
        public static string CategoryNotFound = "Kategori bulunamadı.";
        public static string CategoryNameAlreadyExists = "Bu isimde zaten başka bir kategori var.";
        #endregion

        #region Product
        public static string ProductAdded = "Ürün eklendi.";
        public static string ProductUpdated = "Ürün güncellendi.";
        public static string ProductRemoved = "Ürün silindi.";
        public static string ProductNotFound = "Ürün bulunamadı.";
        public static string ProductNameAlreadyExists = "Bu isimde zaten başka bir ürün var.";
        public static string ProductNotCreateBaseCategory = "Ana kategoriye ürün işlemi yapılamaz.";
        #endregion
    }
}
