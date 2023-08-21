
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

// Connection Built

ConnectionFactory   factory = new ConnectionFactory();
factory.Uri = new("amqps://txgjwwvr:N-FhjvIXseMjQpZ4Awmw9Vg2T_gp3cMQ@moose.rmq.cloudamqp.com/txgjwwvr");

// Connection Activated and Channel Open
using IConnection connection =  factory.CreateConnection();
using IModel channel =  connection.CreateModel();


// Queue Build 

channel.QueueDeclare(queue: "example-queue", exclusive: false);
//Consomer dada kuyruk publisger ile bire bir aynı yapılandırmakda tanımlanmalı bir biriyle haberleşebilmesi için 

//channel.QueueDeclare(queue: "example-queue", exclusive: false,durable: true); // Bu kısımda durable parametresi kuyruk için bir
// configurasyyon olup publish tarafında kuyruğu bu şekilde veri gönderildiyse consume tarafında da bu şekilde belirtilmesi gerekmektedir 
//Yani publisher kalıcı tanımlandıysa consumer dada öyle tanımlanmalı aksi halde sistem hata vericektir 


// Queue read the message
EventingBasicConsumer consumer = new(channel);
var temp = channel.BasicConsume(queue: "example-queue", autoAck: false,consumer);

//channel.BasicCancel(temp); // Bu kısımda  ise queue tarafındaki tüm mesajları red etmek için kullanılacaktır (İlgili kuyruktaki("example-queue") tüm mesajlar reddelicektir )

//channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false); // BU kısımda bu metot ile Fair Dispatch  özelliği ayarlanabilecektir 
//prefetchSize : Alınabilecek en büyük mesaj boyutudur byte cinsindeb , değer 0 ise sınırsız anlamına gelmeketedir 
//prefetchCount : Bir consumer tarafından aynı anda işleme alınabilecek mesaj sayısını belirler 
//global : Tüm consumer larda mı yoksa sadece çağrı yapılan consumerlarda mı geçerli olacağını belirleyecektir 

// channel.BasicConsume(queue: "example-queue", autoAck: false, consumer: consumer);  Buradaki auto ack kısmı Mesaj onaylama sürecini aktifleştiricektir
// Otomatik onaylamayı kapatmış oluyoruz bunu yapınca 



consumer.Received += (sender, e) =>  // delegate
{
    //Kuyruğa gelen mesajın işlendiği yerdir 
    //e.body bize mesajın büyünel verisini getiricektir 
    //e.body.span veya e.body.ToArray() kuyruktaki mesajın byte verisi gelicektir 
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

    channel.BasicAck(e.DeliveryTag, multiple: false); // Başarılı bir şekilde işlendiğine dar uyarı channel Basic.Ack metodu ile gerçekleşir
    //Multiple parametresi ise  true ise DeliveriyTag parametresine sahip bu mesaj ile birlikte bundan önceki mesajlarında işlendiğini onaylar
    //Multiple parametresi false ise sadece bu deliveryTag parametresinin onay bildirisinde bulunacaktır 

    //channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: true); // İşlenmeyen veriler içinin consume tarafının verebileceiği durumlarda kullanılabilir 
    //Multiple parametresi üsteki le aynı olup requeue parametresi ise tekrar queue tarafına verilip verilememesi durumunu patametre olarak almaktadır 

    //channel.BasicReject(deliveryTag: 3, requeue: true); // Kuyruk bazında olmayıp mesaj bazında istemediğimiz mesajın consumer tarafında consume edilmemesi için yapılmaktadır 
};

Console.Read();