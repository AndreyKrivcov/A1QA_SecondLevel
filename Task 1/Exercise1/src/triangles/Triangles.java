
package triangles;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

/**
 * Main class that containes some program logic
 * @author andrey
 */
public class Triangles {

    /**
     * Method that read file and convert it`s data into triangle objects
     * @param path path to the file with coordinates
     * @param delimeter delimeter in the file
     * @param isHeadder tougle that indicates whether this file containe headders or not
     * @return triangles array 
     * @throws IOException 
     */
    private static ArrayList<Triangle> readData(String path, 
                                                String delimeter, 
                                                boolean isHeadder) throws IOException {
        ArrayList<Triangle> data = new ArrayList<>();
        
        // read file
        var fileData = read(path);
        
        // throw exception if file doesn`t containes data
        if(fileData.size() < 1 || (fileData.size() < 2 && isHeadder)) {
            throw new IOException("file doesn`t contains data");
        }
                 
        // Convert lines into triangle objects
        for (int i = (isHeadder ? 1 : 0); i < fileData.size(); i++) {
            var line = fileData.get(i).split(delimeter);
            if(line.length != 6) {
                throw new IOException("File must containe 6 columns {A.x,A.y,B.x,B.y,C.x,C.y}");
            }
            Map<Angle,Point> m = new HashMap<>();
            m.put(Angle.A, new Point(Integer.parseInt(line[0]),Integer.parseInt(line[1])));
            m.put(Angle.B, new Point(Integer.parseInt(line[2]),Integer.parseInt(line[3])));
            m.put(Angle.C, new Point(Integer.parseInt(line[4]),Integer.parseInt(line[5])));
            
            data.add(new Triangle(m));
        }
        
        return data;
    }
    /**
     * Method that read file and return data without of any modifications
     * @param path path to the file
     * @return file data
     * @throws IOException 
     */
    private static ArrayList<String> read(String path) throws IOException {
        
        File file = new File(path);
        
        // Check file
        if(!file.exists()) {
            throw new IOException("File doesn`t exists");
        }
        if(file.isDirectory()) {
            throw new IOException("File expected");
        }
        
        ArrayList<String> list = new ArrayList<>();
        BufferedReader reader=null;
        try{
            reader = new BufferedReader(new FileReader(file));
            
            // Read line by line
            String line;
            while((line = reader.readLine()) != null) {
                list.add(line);            
            }
            
        }catch(IOException ex){
            throw ex;
        }finally{
            if(reader != null) {
                reader.close();
            }
        }
                
        return list;
    }
    /**
     * Method that returns closest to X point of the triangle
     * @param t triangle
     * @return Point identifier
     */
    private static Angle getClosestPointToOX(Triangle t) {
        // Compare Y values for the A and B points
        Angle min = (Math.abs(t.getPoint(Angle.A).Y) < Math.abs(t.getPoint(Angle.B).Y) ? 
                     Angle.A : Angle.B );
        
        // Compare C point with priviouse one 
        return (Math.abs(t.getPoint(min).Y) < Math.abs(t.getPoint(Angle.C).Y) ? min : Angle.C);
    }
    /**
     * Method that returns point for the 90 degree angle
     * @param t triangle
     * @return 90 degree angle identifier
     */
    private static Angle getRightAngle(Triangle t) {
        if(t.getAngle(Angle.A) == 90)
            return Angle.A;
        if(t.getAngle(Angle.B) == 90)
            return Angle.B;
        
        return Angle.C;
    }

    private static void printTrangles(ArrayList<Triangle> triangles) {
        triangles.forEach(x-> {
                Angle closestPointToOX = getClosestPointToOX(x);
                Angle rightAngle = getRightAngle(x);

                System.out.println("Right angle = " + rightAngle.toString() + 
                                   x.getPoint(rightAngle).toString() + 
                                   " | S = " + Double.toString(x.getArea()) + 
                                   " | Closest to OX point = " + 
                                   closestPointToOX.toString() + x.getPoint(closestPointToOX).toString());
    }

    /**
     * Main method
     * @param args arguments (do not used) 
     */
    public static void main(String[] args) {
        // Create console reader
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
        
        try {
            // Read input data
            System.out.print("Enter path to file : ");
            String path = reader.readLine(); 
            System.out.print("Enter delimeter : ");
            String delimeter = reader.readLine();
            System.out.print("Does file contains headders ? (Y/n) : ");
            String headder = reader.readLine();
            boolean isHeadders = headder.toLowerCase().compareTo("y") == 0 || 
                                 headder.compareTo("") == 0;
            
            // Get triangles from the file
            var triangles = readData(path,delimeter,isHeadders);
            // Filter triangles
            triangles.removeIf(x->!x.isRight());
            
            // End program if can`t find any right triangles
            if(triangles.isEmpty()) {
                System.out.println("Can`t find any right triangles");
                return;
            }
            
            // Print data for the finded triangles
            printTrangles(triangles);
            });
            
        } catch (IOException e) {
            System.out.println(e.getMessage());
        } catch (NumberFormatException e) {
            System.out.println("Can`t convert to double " + e.getMessage());
        }
    }
    
}
