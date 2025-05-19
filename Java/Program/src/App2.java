public class App2 {
     public static void main(String[] args) throws Exception {
    System.out.printf("%7d%n", 11111);
    System.out.printf("%-7d%n", 11111);
    System.out.printf("%07d%n", 11111);
    System.out.printf("%,7d%n", 11111);

    System.out.printf("%.1f%n", 123.45f);
    System.out.printf("%1.4f%n", 123.45f);
    System.out.printf("%3.1f%n", 123.45f);
    System.out.printf("%10.2f%n", 123.45f);
    System.out.printf("%010.2f%n", 123.45f);
    System.out.printf("%010f%n", 123.45f);
     }
}
