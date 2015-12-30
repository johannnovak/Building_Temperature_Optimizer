int ready_ = 0;
String input_word;
int input_number;
float x0;
float x1;
float fobj;
int led = 13;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  Serial.write("Boot Done \n");
    pinMode(led, OUTPUT);     

  ready_=1;

}

void loop() {
  // put your main code here, to run repeatedly: 
  
  // send to pc if ready
  if (ready_ > 0)
  {
    Serial.write("12351465");
    Serial.write(",");
    Serial.write("Ready");
    Serial.write('\n');
  }
  else
  {
    fobj = x0 * x0 + x1 * x1; // remplacer par le programme (ex temps de trajet robot) qui revoit un double (si besoin)
    Serial.print(fobj,8);
    Serial.write(",");
    Serial.write("Done");
    Serial.write('\n');
    ready_=1;
  }
  
  // getting numbers
  if (Serial.find("Start"))
 {
  x0=Serial.parseFloat(); // ajouter autant de paramètres que requis (2 param <=> PI)
  x1=Serial.parseFloat();
   ready_=0;
   Serial.println(ready_);
   Serial.println(x0,8);
   Serial.println(x1,8); 
 }
 
}


    

