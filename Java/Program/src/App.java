public class App {
    public static void main(String[] args) throws Exception {
        int intVar = 15;
        double floatVar = 12.345678;
        char charVar = 'A';
        String stringVar = "안녕하세요";

        System.out.printf("%%d: %d%n", intVar);
        System.out.printf("%%o: %o%n", intVar);
        System.out.printf("%%x: %x%n", intVar);
        System.out.printf("%%f: %f%n", floatVar);
        System.out.printf("%%c: %c%n", charVar);
        System.out.printf("%%s: %s%n", stringVar);
        System.out.printf("%%n: ");
        System.out.printf("%n");
        System.out.printf("%n");

        String binary = Integer.toBinaryString(intVar);
        System.out.printf("10진 -> 2진 : %d -> %s\n", intVar, binary);
        System.out.printf("10진 -> 8진 : %d -> %o\n", intVar, intVar);
        System.out.printf("10진 ->16진 : %d -> %x\n", intVar, intVar);

        System.out.printf("%07d%n", 11111);

    }
}
