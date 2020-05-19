
package Triangles;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class Triangles {

    private static ArrayList<Triangle> readData(String path, String delimeter, boolean isHeadder) throws IOException {
        ArrayList<Triangle> data = new ArrayList<>();
        var fileData = read(path);
        
        if(fileData.size() < 1 || (fileData.size() < 2 && isHeadder))
            throw new IOException("file doesn`t contains data");
                 
        for (int i = (isHeadder ? 1 : 0); i < fileData.size(); i++) {
            var line = fileData.get(i).split(delimeter);
            if(line.length != 6)
                throw new IOException("File must containe 6 columns {A.x,A.y,B.x,B.y,C.x,C.y}");
            Map<Angle,Point> m = new HashMap<>();
            m.put(Angle.A, new Point(Integer.parseInt(line[0]),Integer.parseInt(line[1])));
            m.put(Angle.B, new Point(Integer.parseInt(line[2]),Integer.parseInt(line[3])));
            m.put(Angle.C, new Point(Integer.parseInt(line[4]),Integer.parseInt(line[5])));
            
            data.add(new Triangle(m));
        }
        
        return data;
    }
    
    private static ArrayList<String> read(String path) throws IOException {
        
        File file = new File(path);
        
        if(!file.exists())
            throw new IOException("File doesn`t exists");
        if(file.isDirectory())
            throw new IOException("File expected");
        
        ArrayList<String> list = new ArrayList<>();
        BufferedReader reader=null;
        try{
            reader = new BufferedReader(new FileReader(file));
            
            String line;
            while((line = reader.readLine()) != null)
                list.add(line);            
            
        }catch(IOException ex){
            throw ex;
        }finally{
            if(reader != null)
                reader.close();
        }
                
        return list;
    }
    
    private static Angle getClosestPointToOX(Triangle t) {
        Angle min = (Math.abs(t.getPoint(Angle.A).y) < Math.abs(t.getPoint(Angle.B).y) ? 
                     Angle.A : Angle.B );

        return (Math.abs(t.getPoint(min).y) < Math.abs(t.getPoint(Angle.C).y) ? min : Angle.C);
    }
    private static Angle getRightAngle(Triangle t) {
        if(t.getAngle(Angle.A) == 90)
            return Angle.A;
        if(t.getAngle(Angle.B) == 90)
            return Angle.B;
        
        return Angle.C;
    }
    
    public static void main(String[] args) {
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
        
        try {
            System.out.print("Enter path to file : ");
            String path = reader.readLine(); 
            System.out.print("Enter delimeter : ");
            String delimeter = reader.readLine();
            System.out.print("Does file contains headders ? (Y/n) : ");
            String headder = reader.readLine();
            boolean isHeadders = headder.toLowerCase().compareTo("y") == 0 || 
                                 headder.compareTo("") == 0;
            
            var triangles = readData(path,delimeter,isHeadders);
            triangles.removeIf(x->!x.isRight());
            
            if(triangles.isEmpty()) {
                System.out.println("Can`t find any right triangles");
                return;
            }
            
            triangles.forEach(x-> {
                Angle closestPointToOX = getClosestPointToOX(x);
                Angle rightAngle = getRightAngle(x);
                
                System.out.println("Right angle = " + rightAngle.toString() + 
                                   x.getPoint(rightAngle).toString() + 
                                   " | S = " + Double.toString(x.getArea()) + 
                                   " | Closest to OX point = " + 
                                   closestPointToOX.toString() + x.getPoint(closestPointToOX).toString());
            });
            
        } catch (IOException e) {
            System.out.println(e.getMessage());
        } catch (NumberFormatException e) {
            System.out.println("Can`t convert to double " + e.getMessage());
        }
    }
    
}
