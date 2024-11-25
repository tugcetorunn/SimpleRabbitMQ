using RabbitMQ.Client;
using System.Text;

// rabbitmq.client 7 sürümü async metodlar içermekte. 6.4 async metodlar içermemektedir.

// 1. adım - Bağlantı oluşturma
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

// 4. adım - Queue ya mesaj gönderme
// rabbitmq kuyruğa atacağı mesajları byte türünden kabul ediyor.
// byte[] message = Encoding.UTF8.GetBytes("merhaba");
// await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: message); // exchane belirtmedik bu şekilde default olan direct exchange
// işlem yapacak. bu exchange e göre de routing key = queue ismi

for (int i = 0; i < 100; i++)
{
    await Task.Delay(300); // mesajların nasıl geldiğini görebilmek adına ekledik.
    byte[] message = Encoding.UTF8.GetBytes("merhaba " + i);
    await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: message);
}

// bu şekilde consumer ı da oluşturduktan sonra ikisini de aynı anda çağırdığımızda console da arka arkaya mesajların sırayla geldiğini görürüz.

Console.ReadLine();

// rabbitmq.client 6.4 ile yazılırsa;
// using RabbitMQ.Client;
   
// ConnectionFactory factory = new();
// factory.Uri = new("amqps://efithflb:8XLVmInfj3Q37bnvn6yBCUb_1upLf86Y@shrimp.rmq.cloudamqp.com/efithflb");
   
// using IConnection connection = factory.CreateConnection();
// using IModel channel = connection.CreateModel();
   
// channel.QueueDeclare(queue: "example-queue", exclusive: false);