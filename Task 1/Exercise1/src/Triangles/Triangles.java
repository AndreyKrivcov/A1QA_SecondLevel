
package Triangles;

import java.util.HashMap;
import java.util.Map;

public class Triangles {

    public static void main(String[] args) {
        
       Map<Angle,Point> m = new HashMap<>();
       
       m.put(Angle.A, new Point(1,5));
       m.put(Angle.B, new Point(8,-3));
       m.put(Angle.C, new Point(-3,-1));
       
       Triangle t = new Triangle(m);
              
       System.out.println("A" + t.getPoint(Angle.A));
       System.out.println("B" + t.getPoint(Angle.B));
       System.out.println("C" + t.getPoint(Angle.C));
       
      if(t.isRight())
           System.out.println("Right");
      
      System.out.println("Aria is = " + Double.toString(t.getArea()));
      
      System.out.println("Angle A = " + t.getAngle(Angle.A));
      System.out.println("Angle B = " + t.getAngle(Angle.B));
      System.out.println("Angle C = " + t.getAngle(Angle.C));
       
      System.out.println("Angles summ = " +
              (t.getAngle(Angle.A) + t.getAngle(Angle.B) + t.getAngle(Angle.C)));
    }
    
}
