namespace ProtoMath
{
    public delegate void ValueChangedHandler<TSender>(TSender sender); // why always pass the new value when i can access it from within the handler?
    public delegate void ValueChangedHandler(object sender); 
}
