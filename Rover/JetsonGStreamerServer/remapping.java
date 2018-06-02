import java.util.*;
import java.io.*;

public class remapping{
   public static void main(String[] args) throws FileNotFoundException{
      int width = 614;
      int height = 598;
      runCalculations(width);
   }
   
   public static void runCalculations(int width) throws FileNotFoundException{
      PrintStream p1 = new PrintStream(new File("remap_x.txt"));
      PrintStream p2 = new PrintStream(new File("remap_y.txt"));
      int x = 572;
      int y = 592;
      //double[] hello = squareToEllipse(307 / (double)307, 0 / (double)598);
      //System.out.println((int)(hello[0] * 614) + " \t" + (int)(hello[1] * 598) );
      for(int i = -y/2; i < y / 2; i++){
         for (int j = -x/2; j < x / 2; j++){
            double[] lol = squareToEllipse(j / (double)(x/2), i / (double)(y/2));
            p1.print((int)(lol[0] * (x/2) + (x/2)) + " ");
            p2.print((int)(lol[1] * (y/2) + (y/2))  + " ");
         }
         p1.println();
         p2.println(); 
      }
      
//       for (int y = -width; y < width; y++){
//          for (int x = -width; x < width; x++){
//             double longitude = x * Math.PI;
//             double latitude = y * Math.PI / 2.0;
//             System.out.println("longitude: " + longitude);
//             System.out.println("latitude: " + latitude);
//             double p_of_x = Math.cos(latitude) * Math.cos(longitude);
//             double p_of_y = Math.cos(latitude) * Math.sin(longitude);
//             double p_of_z = Math.sin(latitude);
//             System.out.println("P of x: " + p_of_x);
//             System.out.println("P of y: " + p_of_y);
//             System.out.println("P of z: " + p_of_z);
//             double aperture = Math.PI;
//             double atan_top = Math.sqrt(Math.pow(p_of_x, 2) + Math.pow(p_of_z, 2));
//             System.out.println("Atan top: " + atan_top);
//             double radius = 2* Math.atan2(atan_top, p_of_y) / aperture;
//             System.out.println("Radius: " + radius);
//             double theta = Math.atan2(p_of_z, p_of_x);
//             System.out.println("Theta: " + theta);
//             final_x = (radius * Math.cos(theta));
//             final_y = (radius * Math.sin(theta));
//             System.out.print("Final x: " + final_x + " Final y: " + final_y);
//             //System.out.print(final_y + " ");
//             s1.print(final_x + " ");
//             s2.print(final_y + " ");
         //}
        // System.out.println();
//          s1.println();
//          s2.println();
//       }
   }
   
   // Elliptical Grid mapping
   // mapping a square region to a circular disc
   // input: (x,y) coordinates in the square
   // output: (u,v) coordinates in the circle
   public static double[] squareToEllipse(double x, double y){
      double u = x * Math.sqrt(1.0 - y*y/2.0);
      double v = y * Math.sqrt(1.0 - x*x/2.0);
      double[] coordinates = {u, v};
      return coordinates;
   }
}  