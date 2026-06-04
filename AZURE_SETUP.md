# 🚀 Azure Dağıtım Konfigürasyonu

## Azure Portal - App Service Configuration

### Connection String Ekleme Adımları:
1. Azure Portal → MUOffice App Service → Configuration
2. **Connection strings** bölümüne ekle:

```
DefaultConnection = Server=tcp:YOUR_SERVER.postgres.database.azure.com,5432;Database=WebProgProjc;Username=YOUR_USERNAME@YOUR_SERVER;Password=YOUR_PASSWORD;Ssl Mode=Require;Trust Server Certificate=false;
```

### Environment Variable:
```
ASPNETCORE_ENVIRONMENT = Production
```

---

## PostgreSQL Sunucu Bilgileri

Şu bilgileri Azure Portal'dan bulabilirsiniz:
- **Server**: `{YOUR_SERVER}.postgres.database.azure.com`
- **Username**: `{YOUR_USERNAME}@{YOUR_SERVER}` (format önemli!)
- **Password**: Oluşturduğunuz şifre
- **Database**: `WebProgProjc` (oluşturmanız gerekebilir)

---

## Veritabanı İlk Ayarı

Azure Cloud Shell'de çalıştırın:
```bash
# PostgreSQL'e bağlan
psql -h YOUR_SERVER.postgres.database.azure.com -U YOUR_USERNAME@YOUR_SERVER -d postgres

# Veritabanı oluştur
CREATE DATABASE "WebProgProjc";

# Çık
\q
```

---

## GitHub Actions Workflow Durumu
- ✅ Production environment ayarlandı
- ✅ Build ve publish optimize edildi
- ✅ Azure deployment yapılandırıldı

Şimdi `main` branch'e push yapın, GitHub Actions otomatik deploy edecek!
