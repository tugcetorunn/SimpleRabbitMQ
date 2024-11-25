

// 1. adım - Bağlantı oluşturma
using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();

// factory.HostName = "guest"; // varsa hostname ve password bilgileri ile de bağlanılabilir.
// factory.Password = "guest";

factory.Uri = new("amqps://efithflb:8XLVmInfj3Q37bnvn6yBCUb_1upLf86Y@shrimp.rmq.cloudamqp.com/efithflb"); // amqp  ile bağlanıyoruz.

// 2. adım - Bağlantıyı aktifleştirme ve kanal açma

using IConnection connection = factory.CreateConnectionAsync().Result; // IConnection IDisposable interface i olduğu için using ile işaretliyoruz ki işlem
                                                                       // tamamlandıktan sonra bu nesne dispose adilip bellekte temizlenmiş olsun. 
using IChannel channel = connection.CreateChannelAsync().Result;

// 3. adım - Queue oluşturma

await channel.QueueDeclareAsync(queue: "example-queue", exclusive: false); // durable: kuyruktaki mesajların kalıcığıyla alakalı bir configuration sağlar.
                                                                           // exclusive: bu kuyruğun özel olup olmadığını yani bu kuyrukta birden fazla işlem yapıp
                                                                           // yapamayacağımızı belirlediğimiz bir özellik. default true dur true olduğunda bu
                                                                           // bağlantının dışında başka bir bağlantı bu kuyruğu kullanamaz. başka bir channelda
                                                                           // bu kuyruğa bağlı bir işlem gerçekleştiremeyiz. publisherla biz bu kuyruğa bir mesaj
                                                                           // göndericez fakat aynı zamanda consumerla da bu kuyruğa bağlanıp buradaki mesajları
                                                                           // tüketmemiz gerektiği için false yapmalıyız. exclusive true olan queue o bağlantıya 
                                                                           // özel oluşturulur. daha sonra bu queue consumer oluşamadan silinir. 
                                                                           // autoDelete: kuyruğun içindeki tüm mesajlar tüketildikten sonra kuyruğun otomatik
                                                                           // silinip silinmeyeceğini belirlediğimiz özellik.

// 4. adım - Kuyruğa mesaj gönderme
// rabbitmq kuyruğa atacağı mesajları byte türünden kabul ediyor.

byte[] message = Encoding.UTF8.GetBytes("merhaba");
await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: message); // exchane belirtmedik bu şekilde default olan direct exchange
                                                                                           // işlem yapacak. bu exchange e göre de routing key = queue ismi

Console.ReadLine();
