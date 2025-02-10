
Microservis mimarisi yaklaşımıyla Net Core 8 ile  Sipariş, Stok ve Bildirim modülleri geliştirilmiştir. 
Message Broker olarak RabbitMQ kullanılarak servisler arasındaki haberleşme asekron olarak tercih edilmiştir. Böylece servisler arası bağımsızlık sağlanmıştır ve veri kaybı engellenmiştir.

RabbitMQ Clıud - https://www.cloudamqp.com/

PostgreSQL lokalde kurulmuştur.

Mimaride Outbox ve Repository pattern kullanılmıştır.

![Screenshot 2025-02-10 110734](https://github.com/user-attachments/assets/f3026eea-0cbd-4b1f-a9a5-4774fece2801)

![Screenshot 2025-02-10 110753](https://github.com/user-attachments/assets/1b37277f-9546-48df-a3c3-45a403b9bcc3)

Not : Proje kapsamında 2. yöntem olarak HttpClient kullanarak servisler arası iletişim kurmak için endpointlerde tanımlanmıştır. Circuit Breaker kullanarak bu yömntemle de geliştirilebilir. Fakat bu yöntemin dezavantajı servisler arası bağımlılık oluşturmasıdır.
