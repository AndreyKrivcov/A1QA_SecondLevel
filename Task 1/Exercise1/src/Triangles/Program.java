package Triangles;

import java.util.Map;

class Point {
    public Point(double x, double y) {
        this.x = x;
        this.y= y;
    }
    public final double x,y;
        
    @Override
    public String toString(){
        return "("+Double.toString(x) + ";" + Double.toString(y) + ")";
    }   
}

class Triangle {
    public Triangle(Map<Angle,Point> data) {
        coordinates = data;
    }
    
    private final Map<Angle,Point> coordinates;

    private double cos(Angle angle) {
        double b=0, c=0, a=0;
        
        switch(angle){
            case A :{
                b = getLength(coordinates.get(Angle.A),coordinates.get(Angle.B));
                c = getLength(coordinates.get(Angle.A),coordinates.get(Angle.C));
                a = getLength(coordinates.get(Angle.B),coordinates.get(Angle.C));
            } break;
            case B :{
                b = getLength(coordinates.get(Angle.B),coordinates.get(Angle.A));
                c = getLength(coordinates.get(Angle.B),coordinates.get(Angle.C));
                a = getLength(coordinates.get(Angle.A),coordinates.get(Angle.C));
            } break;
            case C :{
                b = getLength(coordinates.get(Angle.C),coordinates.get(Angle.B));
                c = getLength(coordinates.get(Angle.C),coordinates.get(Angle.A));
                a = getLength(coordinates.get(Angle.B),coordinates.get(Angle.A));
            } break;
        }
        
        double ans = (Math.pow(b,2) + Math.pow(c,2) - Math.pow(a,2))/(2*b*c);
        return ans ;
    }
    
    private double getLength(Point a, Point b) {
        return Math.sqrt(Math.pow(a.x - b.x,2) + Math.pow(a.y - b.y,2));
    }
    
    public boolean isRight() {        
        return coordinates.entrySet().stream().anyMatch((entry) -> (getAngle(entry.getKey()) == 90.0));
    }
    
    public double getArea() {
        double a = getLength(coordinates.get(Angle.A), coordinates.get(Angle.B));
        double b = getLength(coordinates.get(Angle.A), coordinates.get(Angle.C));
        
        return (a * b * Math.sqrt(1-Math.pow(cos(Angle.A),2)))/2;
    }
    
    public Point getPoint(Angle angle) {
        return coordinates.get(angle);
    }
    
    public double getAngle(Angle angle) {
        double ans = Math.acos(cos(angle))*180/Math.PI;
        return Math.round(ans * 100000)/100000.0;
    }
}

enum Angle {
    A,
    B,
    C
}
