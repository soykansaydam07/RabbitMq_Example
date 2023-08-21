// Connection Build
using System.Text;
using System;
using RabbitMQ.Client;
using System.Threading.Tasks;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://txgjwwvr:N-FhjvIXseMjQpZ4Awmw9Vg2T_gp3cMQ@moose.rmq.cloudamqp.com/txgjwwvr");


// Connection activate
// Buradaki using yapısı IDisposeable olduğu için intance ile ilgili konu bitince bellekte temizlenmiş olsun (Optimizasyon)
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// Queue Build
channel.QueueDeclare(queue: "example-queue", exclusive: false);

//channel.QueueDeclare(queue: "example-queue", exclusive: false,durable: true); // Bu kısımda durable parametresi kuyruk için bir
                                                                              // configurasyyon olup publish tarafında kuyruğu kalıcı hale getirmektedir 

// Queue send the message

//Rabbitmq kuruğa atılıcak mesajları byte türünden kabul etmektedir Haliyle mesajları byte a dönüştürmek gerekir 

//byte[] message = Encoding.UTF8.GetBytes("Merhaba");
//channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);

//IBasicProperties properties = channel.CreateBasicProperties();
//properties.Persistent = true; // Bu kısımda ise ilgili kuyruktaki mesajların kalıcı olarak sağlanması için bir konfigure yapılandırılmasıdır bu ve bir üst satır

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);

    //channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message,basicProperties:properties);// Bu kısımda da publish edilen veride bu configure
                                                                                                              // tarafına ait verinin kullanılması gerekmektedir 
}

Console.Read();