using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// rabbitmq.client 6.4 versiyonu ile yapalım.

// 1. adım - Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://efithflb:8XLVmInfj3Q37bnvn6yBCUb_1upLf86Y@shrimp.rmq.cloudamqp.com/efithflb");

// 2. adım - Bağlantıyı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// 3. adım - Queue oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false); // parametreler de birebir aynı olması false sa false gitli yoksa tutarsızlık olur
                                                                // hata meydana gelir.

// 4. adım - Queue dan mesaj okuma
// kuyruktan okuma yapmamız için bu channel üzerinde bir event operasyonu yapmamız gerek.
EventingBasicConsumer consumer = new(channel); // bu class parametre olarak IModel türünden obje ister. çünkü işlemlerini bu channel üzerinden yapacak.
// consumer ismindeki bu instance üzerinde bu channel da bildireceğimiz kuyrukta bir mesaj olursa onu receive (?consume) edeceğiz (alıp işleyeceğiz). 
channel.BasicConsume(queue: "example-queue", autoAck: false, consumer: consumer); // autoack: kuyruktan mesajı aldığımızda silinip silinmemesini
                                                                                  // belirlediğimiz özellik.
                                                                                  
consumer.Received += (sender, e) => // mesaj geldiği anda yakalaması (receive) için recieved kullanılır. 
// delegate, bir yöntemi bir değer olarak ele almanıza, yöntemi bir değişkene atamanıza, diğer yöntemlere parametre olarak geçirmenize, bir koleksiyona
// eklemenize vb. olanak tanıyan nesnedir. += () =>
// https://stackify.com/c-delegates-definition-types-examples/
{
    // kuyruğa gelen mesajın işlendiği yerdir.
    // e.Body: kuyruktaki mesajın verisini büyünsel olarak getirir.
    // e.Body.Span veya e.Body.ToArray: kuyruktaki mesajın byte verisini getirir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.ReadLine();

// consumer.Received += Consumer_Received;
   
// void Consumer_Received(object? sender, BasicDeliverEventArgs e)
// {
//     throw new NotImplementedException();
// } // bu şekilde metodlaştırılabilir, .net otomatik oluşturdu.