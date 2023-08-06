
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

// Queue read the message
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue",false,consumer);
consumer.Received += (sender, e) =>  // delegate
{
    //Kuyruğa gelen mesajın işlendiği yerdir 
    //e.body bize mesajın büyünel verisini getiricektir 
    //e.body.span veya e.body.ToArray() kuyruktaki mesajın byte verisi gelicektir 
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.Read();