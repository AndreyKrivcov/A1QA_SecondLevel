package triangles;

import java.util.Map;

/**
 * Class that represent triangle object
 * @author andrey
 */
class Triangle {
    /**
     * Constructor
     * @param data input coordinates
     */
    public Triangle(Map<Angle,Point> data) {
        COORDINATES = data;
    }
    
    private final Map<Angle,Point> COORDINATES;

    /**
     * Method that calculates cosinus using coordinates
     * the formula comes from formula of length of the opposite to the selected angle catit
     * A^2 = B^2+C^2-2BCCos(a)
     * Cos(a) = (B^2+C^2-A^2)/(2BC)
     * @param angle andge point
     * @return cosinus value
     */
    private double cos(Angle angle) {
        double b;
        double c;
        double a;
        
        switch(angle){
            case A :{
                b = getLength(COORDINATES.get(Angle.A),COORDINATES.get(Angle.B));
                c = getLength(COORDINATES.get(Angle.A),COORDINATES.get(Angle.C));
                a = getLength(COORDINATES.get(Angle.B),COORDINATES.get(Angle.C));
            } break;
            case B :{
                b = getLength(COORDINATES.get(Angle.B),COORDINATES.get(Angle.A));
                c = getLength(COORDINATES.get(Angle.B),COORDINATES.get(Angle.C));
                a = getLength(COORDINATES.get(Angle.A),COORDINATES.get(Angle.C));
            } break;
            case C :{
                b = getLength(COORDINATES.get(Angle.C),COORDINATES.get(Angle.B));
                c = getLength(COORDINATES.get(Angle.C),COORDINATES.get(Angle.A));
                a = getLength(COORDINATES.get(Angle.B),COORDINATES.get(Angle.A));
            } break;
            default : 
            {
                b = 0;
                c = 0;
                a = 0;
            }
        }
        
        return((Math.pow(b,2) + Math.pow(c,2) - Math.pow(a,2))/(2*b*c));
    }
    
    /**
     * Length of the line
     * |AB| = sqrt((A.x-B.x)^2 + (A.y-B.y)^2)
     * @param a point 1
     * @param b point 2
     * @return line lenght
     */
    private double getLength(Point a, Point b) {
        return Math.sqrt(Math.pow(a.X - b.X,2) + Math.pow(a.Y - b.Y,2));
    }
    
    /**
     * Method that indicates whether this triangle right or not.
     * It is right triangle if it containes a 90 degree angle
     * @return true if this is right triangle
     */
    public boolean isRight() {        
        return COORDINATES.entrySet().stream()
                .anyMatch((entry) -> (getAngle(entry.getKey()) == 90.0));
    }
    
    /**
     * Method that calculates triangle aria using angle A and two catits
     * @return 
     */
    public double getArea() {
        double a = getLength(COORDINATES.get(Angle.A), COORDINATES.get(Angle.B));
        double b = getLength(COORDINATES.get(Angle.A), COORDINATES.get(Angle.C));
        
        return (a * b * Math.sqrt(1-Math.pow(cos(Angle.A),2)))/2;
    }
    
    /**
     * Method that return selected point
     * @param angle
     * @return 
     */
    public Point getPoint(Angle angle) {
        return COORDINATES.get(angle);
    }
    
    /**
     * Method that calculate angle in degree for the given point
     * @param angle
     * @return 
     */
    public double getAngle(Angle angle) {
        double ans = Math.acos(cos(angle))*180/Math.PI;
        return Math.round(ans * 100000)/100000.0;
    }
}

/**
 * Class that represent point
 * @author andrey
 */
class Point {
    public Point(double x, double y) {
        this.X = x;
        this.Y= y;
    }
    
    public final double X;
    public final double Y;
        
    /**
     * Methot that convert point coordinates to string in format (X;Y)
     * @return point coordinates
     */
    @Override
    public String toString(){
        return "("+Double.toString(X) + ";" + Double.toString(Y) + ")";
    }   
}

/**
 * Angle types
 * @author andrey
 */
enum Angle {
    A,
    B,
    C
}
