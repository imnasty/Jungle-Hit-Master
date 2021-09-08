#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("xN+e2XPKkw70ezYnElrWAKqTop1SWohrvqqsOFbWuEoBAhqJNB9atczLyM3Jys+j7vTKzMnLycDLyM3Je/j5//DTf7F/Dpqd/PjJeAvJ0/9HDYpiFyud9jKAts0hW8cAgQaSMfz5+nv49vnJe/jz+3v4+PkdaFDw78nt//qs/frq9LiJiZWc2auWlo2XndmalpedkI2QlpeK2Zaf2YyKnKBe/PCF7rmv6OeNKk5y2sK+WiyWiZWc2auWlo3ZurjJ5+70yc/JzcuGuFFhACgzn2XdkugpWkId4tM65tmYl53ZmpyLjZCfkJqYjZCWl9mJ2Zaf2Y2RnNmNkZyX2ZiJiZWQmphO4kRqu93r0z725E+0ZaeaMbJ57pCfkJqYjZCWl9m4jI2RlouQjYDIgNmYioqMlJyK2ZiampyJjZiXmpxsZ4P1Xb5yoi3vzsoyPfa0N+2QKNXZmpyLjZCfkJqYjZzZiZaVkJqAjZGWi5CNgMjvye3/+qz9+ur0uInZurjJe/jbyfT/8NN/sX8O9Pj4+P/J9v/6rOTq+PgG/fzJ+vj4Bsnkm5Wc2YqNmJedmIud2Y2ci5SK2Zh2iniZP+Ki8NZrSwG9sQmZwWfsDOZoIue+qRL8FKeAfdQSz1uutawVvIfmtZKpb7hwPY2b8ul6uH7Kc3iOjteYiYmVnNealpTWmImJlZyamNbJeDr/8dL/+Pz8/vv7yXhP43hK8dL/+Pz8/vv47+eRjY2JisPW1o7dGxIoTokm9rwY3jMIlIEUHkzu7sno//qs/fPq87iJiZWc2bCXmtfI/hWEwHpyqtkqwT1IRmO285IG0gUgz4Y4fqwgXmBAy7sCISyIZ4dYq4uYmo2QmpzZio2YjZyUnJeNitfJ38nd//qs/fLq5LiJiZWc2bqci43XuV8OvrSG8afJ5v/6rOTa/eHJ7/ZkxArSsNHjMQc3TED3IKflLzLEee3SKZC+bY/wBw2SdNe5Xw6+tIbJe/1CyXv6Wln6+/j7+/j7yfT/8MrPo8mbyPLJ8P/6rP3/6vusqsjqsCGPZsrtnFiObTDU+/r4+fhae/iJlZzZupyLjZCfkJqYjZCWl9m4jPGnyXv46P/6rOTZ/Xv48cl7+P3JUSWH28wz3Cwg9i+SLVvd2ugOWFXmfHp84mDEvs4LUGK5d9UtSGnrIZ528U3ZDjJV1dmWiU/G+Ml1Tro2q5yVkJiXmpzZlpfZjZGQitmanIv9/+r7rKrI6sno//qs/fPq87iJifT/8NN/sX8O9Pj4/Pz5+nv4+Pml03+xfw70+Pj8/PnJm8jyyfD/+qz/+qzk9/3v/e3SKZC+bY/wBw2SdJ3M2uyy7KDkSm0OD2VnNqlDOKGpcuBwJwCylQz+UtvJ+xHhxwGp8CqDyXv4j8n3//qs5Pb4+Ab9/fr7+M9gtdSBThR1YiUKjmILjyuOybY4lZzZsJea18jfyd3/+qz98urkuIlIyaEVo/3LdZFKduQnnIoGnqecRUzDVA329/lr8kjY79eNLMX0IpvvjZCfkJqYjZzZm4DZmJeA2YmYi40w4IsMpPcshqZiC9z6Q6x2tKT0CDmayo4Ow/7VrxIj9tj3I0OK4LZMqVNzLCMdBSnw/s5JjIzY");
        private static int[] order = new int[] { 56,24,7,34,50,14,12,43,52,12,17,45,41,59,45,40,18,44,45,59,29,42,55,40,25,36,37,35,53,41,51,58,46,44,58,55,46,56,52,55,50,44,54,49,54,52,49,47,49,51,54,52,53,53,58,56,58,58,59,59,60 };
        private static int key = 249;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
