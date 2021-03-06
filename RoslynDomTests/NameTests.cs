﻿using System.Linq;
using ApprovalTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynDom;
using RoslynDom.CSharp;

namespace RoslynDomTests
{
    [TestClass]
    public class NameTests
    {
        // Test categories
        private const string SimpleNameCategory = "SimpleName";
        private const string NestedNameCategory = "NestedName";
        private const string KeywordNameCategory = "KeywordName";
        private const string QualifiedNameCategory = "QualifiedName";
        private const string OuterNameCategory = "OuterName";
        private const string NamespaceNameCategory = "NamespaceName";
        private const string ReferencedTypeNamingCategory = "ReferencedTypeNameing";

        #region simple name methods
        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Root_is_named_root()
        {
            var csharpCode = @"
                        using System.Diagnostics.Tracing;
                        namespace testing.Namespace1
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("<root>", root.Name);
        }

        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_using_name_multipart()
        {
            var csharpCode = @"
                        using System.Diagnostics.Tracing;
                        namespace testing.Namespace1
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("System.Diagnostics.Tracing", root.UsingDirectives.First().Name);
        }

        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_using_name()
        {
            var csharpCode = @"
                        using System;
                        namespace testing.Namespace1
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("System", root.UsingDirectives.First().Name);
        }

        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_namespace_name()
        {
            var csharpCode = @"
                        namespace testing.Namespace1
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            var ns = root.DescendantNamespaces.Last();
            var symbol = ((IRoslynHasSymbol)ns).Symbol;
            Assert.AreEqual("testing", ns.Namespace);
            Assert.AreEqual("testing.Namespace1", ns.QualifiedName);
            Assert.AreEqual("Namespace1", symbol.MetadataName, "meta");
            Assert.AreEqual("Namespace1", ns.Name);
        }

        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_class_name()
        {
            var csharpCode = @"
                        public class MyClass
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            var cl = root.Classes.First();
            var symbol = ((IRoslynHasSymbol )cl).Symbol;
            Assert.AreEqual("MyClass", cl.Name);
            Assert.AreEqual("MyClass", symbol.MetadataName);
            Assert.AreEqual("MyClass", symbol.Name);
        }

        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_enums_name()
        {
            var csharpCode = @"
                        public enum MyEnum
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            var en = root.Enums.First();
            var symbol = ((IRoslynHasSymbol)en).Symbol;
            Assert.AreEqual("MyEnum", en.Name);
            Assert.AreEqual("MyEnum", symbol.MetadataName);
            Assert.AreEqual("MyEnum", symbol.Name);
        }


        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_struct_name()
        {
            var csharpCode = @"
                        public struct MyStruct
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            var st = root.Structures.First();
            var symbol = ((IRoslynHasSymbol)st).Symbol;
            Assert.AreEqual("MyStruct", st.Name);
            Assert.AreEqual("MyStruct", symbol.MetadataName);
            Assert.AreEqual("MyStruct", symbol.Name);
        }


        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_interface_name()
        {
            var csharpCode = @"
                        public interface MyInterface
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
           // Approvals.Verify(root);
            var inter = root.Interfaces.First();
            var symbol = ((IRoslynHasSymbol)inter).Symbol;
            Assert.AreEqual("MyInterface", inter.Name);
            Assert.AreEqual("MyInterface", symbol.MetadataName);
            Assert.AreEqual("MyInterface", symbol.Name);
        }

        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_field_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myField; }";
            var root = RDom.CSharp.Load(csharpCode);
            var fld = root.Classes.First().Fields.First();
            var symbol = ((IRoslynHasSymbol)fld).Symbol;
            Assert.AreEqual("myField", fld.Name);
            Assert.AreEqual("myField", symbol.MetadataName);
            Assert.AreEqual("myField", symbol.Name);
        }

        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_multi_field_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myField, myField2; }";
            var root = RDom.CSharp.Load(csharpCode);
            var fld = root.Classes.First().Fields.First();
            var fld2 = root.Classes.First().Fields.Last();
            var symbol = ((IRoslynHasSymbol)fld).Symbol;
            var symbol2 = ((IRoslynHasSymbol)fld2).Symbol;
            Assert.AreEqual("myField", fld.Name);
            Assert.AreEqual("myField2", fld2.Name);
            Assert.AreEqual("myField", symbol.MetadataName);
            Assert.AreEqual("myField2", symbol2.MetadataName);
            Assert.AreEqual("myField", symbol.Name);
            Assert.AreEqual("myField2", symbol2.Name);
        }

        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_property_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myProperty { get; } }";
            var root = RDom.CSharp.Load(csharpCode);
            var pr = root.Classes.First().Properties.First();
            var symbol = ((IRoslynHasSymbol)pr).Symbol;
            Assert.AreEqual("myProperty", pr.Name);
            Assert.AreEqual("myProperty", symbol.MetadataName);
            Assert.AreEqual("myProperty", symbol.Name);
        }

        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_method_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myMethod(int x) { return x; } }";
            var root = RDom.CSharp.Load(csharpCode);
            var me = root.Classes.First().Methods.First();
            var symbol = ((IRoslynHasSymbol)me).Symbol;
            Assert.AreEqual("myMethod", me.Name);
            Assert.AreEqual("myMethod", symbol.MetadataName);
            Assert.AreEqual("myMethod", symbol.Name);
        }

        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_namespace_for_null()
        {
            var result = RoslynDomUtilities.GetNamespace(null);
            Assert.IsTrue(result == "");
        }

        [TestMethod, TestCategory(KeywordNameCategory)]
        public void Can_get_namespace_qualified_name()
        {
            var csharpCode = @"
                        namespace Namespace2
                        {
                        namespace testing.Namespace1
                            { }
                        }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("Namespace2.testing.Namespace1", root.DescendantNamespaces.Last().QualifiedName);
        }

        #endregion

        #region nested name tests
        [TestMethod, TestCategory(SimpleNameCategory)]
        public void Can_get_nestedType_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public class MyNestedClass {  } }";
            var root = RDom.CSharp.Load(csharpCode);
            var nestedType = root.Classes.First().Classes.First();
            var symbol = ((IRoslynHasSymbol)nestedType).Symbol;
            Assert.AreEqual("MyNestedClass", nestedType.Name);
            Assert.AreEqual("MyNestedClass", symbol.MetadataName);
            Assert.AreEqual("MyNestedClass", symbol.Name);
        }

        [TestMethod, TestCategory(NestedNameCategory)]
        public void Can_get_nested_namespace_name()
        {
            var csharpCode = @"
                        namespace Namespace2
                        {
                        namespace testing.Namespace1
                        { }
                        }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            var ns = root.ChildNamespaces.First();
            var symbol = ((IRoslynHasSymbol)ns).Symbol;
            Assert.AreEqual("Namespace1", root.DescendantNamespaces.Last().Name);
            Assert.AreEqual("Namespace2.testing.Namespace1", root.DescendantNamespaces.Last().QualifiedName);
        }

        [TestMethod, TestCategory(NestedNameCategory)]
        public void Can_get_nested_class_name()
        {
            var csharpCode = @"
namespace Namespace1
{
                        public class MyClass
                            { }
                        
}";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("MyClass", root.Namespaces.First().Classes.First().Name);
        }

        [TestMethod, TestCategory(NestedNameCategory)]
        public void Can_get_nested_enums_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public enum MyEnum
                            { }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("MyEnum", root.Classes.First().Enums.First().Name);
        }

        [TestMethod, TestCategory(NestedNameCategory)]
        public void Can_get_nested_struct_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public struct MyStruct
                            { }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("MyStruct", root.Classes.First().Structures.First().Name);
        }


        [TestMethod, TestCategory(NestedNameCategory)]
        public void Can_get_nested_interface_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public interface MyInterface
                            { }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("MyInterface", root.Classes.First().Interfaces.First().Name);
        }

        [TestMethod, TestCategory(NestedNameCategory)]
        public void Can_get_nested_field_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                            public class MyNestedClass
                            { 
                                public int myField; 
                            }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("myField", root.Classes.First().Classes.First().Fields.First().Name);
        }

        [TestMethod, TestCategory(NestedNameCategory)]
        public void Can_get_nested_property_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                            public class MyNestedClass
                            { 
                                public int myProperty { get; } 
                            }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("myProperty", root.Classes.First().Classes.First().Properties.First().Name);
        }

        [TestMethod, TestCategory(NestedNameCategory)]
        public void Can_get_nested_method_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                            public class MyNestedClass
                            { 
                                public int myMethod(int x) { return x; } 
                            }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("myMethod", root.Classes.First().Classes.First().Methods.First().Name);
        }

        [TestMethod, TestCategory(NestedNameCategory)]
        public void Can_get_nested_nestedType_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                            public class MyNestedClass
                            { 
                                public class MyNestedNestedClass {}
                            }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("MyNestedNestedClass", root.Classes.First().Classes.First().Classes.First().Name);
        }



        #endregion

        #region keyword name tests
        [TestMethod, TestCategory(KeywordNameCategory)]
        public void Can_get_keyword_namespace_name()
        {
            var csharpCode = @"
                        using System.Diagnostics.Tracing;
                        namespace @class
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("class", root.ChildNamespaces.First().Name);
        }

        [TestMethod, TestCategory(KeywordNameCategory)]
        public void Can_get_keyword_class_name()
        {
            var csharpCode = @"
                        public class @namespace
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("namespace", root.Classes.First().Name);
        }

        [TestMethod, TestCategory(KeywordNameCategory)]
        public void Can_get_keyword_enums_name()
        {
            var csharpCode = @"
                        public enum @class
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("class", root.Enums.First().Name);
        }


        [TestMethod, TestCategory(KeywordNameCategory)]
        public void Can_get_keyword_struct_name()
        {
            var csharpCode = @"
                        public struct @class
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("class", root.Structures.First().Name);
        }

        [TestMethod, TestCategory(KeywordNameCategory)]
        public void Can_get_keyword_interface_name()
        {
            var csharpCode = @"
                        public interface @class
                            { }
                        ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("class", root.Interfaces.First().Name);
        }

        [TestMethod, TestCategory(KeywordNameCategory)]
        public void Can_get_keyword_field_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int @class; }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("class", root.Classes.First().Fields.First().Name);
        }

        [TestMethod, TestCategory(KeywordNameCategory)]
        public void Can_get_keyword_property_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int @class { get; } }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("class", root.Classes.First().Properties.First().Name);
        }

        [TestMethod, TestCategory(KeywordNameCategory)]
        public void Can_get_keyword_method_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int @class(int x) { return x; } }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("class", root.Classes.First().Methods.First().Name);
        }

        [TestMethod, TestCategory(KeywordNameCategory)]
        public void Can_get_keyword_nestedType_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public class @public {  } }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("public", root.Classes.First().Classes.First().Name);
        }


        #endregion

        #region qualified name tests

        [TestMethod, TestCategory(QualifiedNameCategory)]
        public void Can_get_class_qualified_name()
        {
            var csharpCode = @"
namespace Namespace1
{
                        public class MyClass
                            { }
                        
}";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("Namespace1.MyClass", root.Namespaces.First().Classes.First().QualifiedName);
        }

        [TestMethod, TestCategory(QualifiedNameCategory)]
        public void Can_get_class_qualified_name_without_namespace()
        {
            var csharpCode = @"
                        public class MyClass
                            { }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("MyClass", root.Classes.First().QualifiedName);
        }

        [TestMethod, TestCategory(QualifiedNameCategory)]
        public void Can_get_qualified_enums_name()
        {
            var csharpCode = @"
 namespace Namespace1
{                       public enum MyEnum
                            { }
  }                      ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("Namespace1.MyEnum", root.Namespaces.First().Enums.First().QualifiedName);
        }


        [TestMethod, TestCategory(QualifiedNameCategory)]
        public void Can_get_qualified_struct_name()
        {
            var csharpCode = @"
namespace Namespace1
{                        public struct MyStruct
                            { }
 }                       ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("Namespace1.MyStruct", root.Namespaces.First().Structures.First().QualifiedName);
        }


        [TestMethod, TestCategory(QualifiedNameCategory)]
        public void Can_get_qualified_interface_name()
        {
            var csharpCode = @"
namespace Namespace1
{                        public interface MyInterface
                            { }
}                        ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("Namespace1.MyInterface", root.Namespaces.First().Interfaces.First().QualifiedName);
        }

        [TestMethod, TestCategory(QualifiedNameCategory)]
        public void Get_nestedType_qualified_name()
        {
            var csharpCode = @"
namespace Namespace1
{
                        public class MyClass
                        { 
                            public class MyNestedClass
                            { 
                                public class MyNestedNestedClass {}
                            }
                        }
}";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("Namespace1.MyClass.MyNestedClass.MyNestedNestedClass",
                root.Namespaces.First().Classes.First().Classes.First().Classes.First()
                .QualifiedName);
        }
        #endregion

        #region outer name tests
//        [TestMethod, TestCategory(OuterNameCategory)]
//        public void Can_get_outer_namespace_name()
//        {
//            var csharpCode = @"
//                        namespace Namespace2
//                        {
//                            namespace testing.Namespace1
//                            { }
//                        }
//                        ";
//            var root = RDomCSharp.Factory.GetRootFromString(csharpCode);
//            Assert.AreEqual("Namespace2.testing.Namespace1", 
//                root.ChildNamespaces.First().ChildNamespaces.First().ChildNamespaces.First().OuterName);
//        }

//        [TestMethod, TestCategory(OuterNameCategory)]
//        public void Can_get_outer_namespace_empty_name()
//        {
//            var csharpCode = @"
//                        namespace Namespace1
//                            { }
//                        ";
//            var root = RDomCSharp.Factory.GetRootFromString(csharpCode);
//            Assert.AreEqual("Namespace1", root.ChildNamespaces.First().OuterName);
//        }


        [TestMethod, TestCategory(OuterNameCategory)]
        public void Can_get_outer_class_name()
        {
            var csharpCode = @"
namespace Namespace1
{
                        public class MyClass
                            { }
                        
}";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("Namespace1.MyClass", root.Namespaces.First().Classes.First().OuterName);
        }

        [TestMethod, TestCategory(OuterNameCategory)]
        public void Can_get_outer_enums_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public enum MyEnum
                            { }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("MyClass+MyEnum", root.Classes.First().Enums.First().OuterName);
        }

        [TestMethod, TestCategory(OuterNameCategory)]
        public void Can_get_outer_struct_name()
        {

            var csharpCode = @"
                        public class MyClass
                        { 
                        public struct MyStruct
                            { }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("MyClass+MyStruct", root.Classes.First().Structures.First().OuterName);
        }

        [TestMethod, TestCategory(OuterNameCategory)]
        public void Can_get_outer_interface_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public interface MyInterface
                            { }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("MyClass+MyInterface", root.Classes.First().Interfaces.First().OuterName);
        }

//        [TestMethod, TestCategory(OuterNameCategory)]
//        public void Can_get_outer_field_name()
//        {
//            var csharpCode = @"
//                        public class MyClass
//                        { 
//                            public class MyNestedClass
//                            { 
//                                public int myField; 
//                            }
//                        }";
//            var root = RDomCSharp.Factory.GetRootFromString(csharpCode);
//            Assert.AreEqual("MyClass+MyNestedClass.myField", root.Classes.First().Classes.First().Fields.First().OuterName);
//        }

//        [TestMethod, TestCategory(OuterNameCategory)]
//        public void Can_get_outer_property_name()
//        {
//            var csharpCode = @"
//                        public class MyClass
//                        { 
//                            public class MyNestedClass
//                            { 
//                                public int myProperty { get; } 
//                            }
//                        }";
//            var root = RDomCSharp.Factory.GetRootFromString(csharpCode);
//            Assert.AreEqual("MyClass+MyNestedClass.myProperty", root.Classes.First().Classes.First().Properties.First().OuterName);
//        }

//        [TestMethod, TestCategory(OuterNameCategory)]
//        public void Can_get_outer_method_name()
//        {
//            var csharpCode = @"
//                        public class MyClass
//                        { 
//                            public class MyNestedClass
//                            { 
//                                public int myMethod(int x) { return x; } 
//                            }
//                        }";
//            var root = RDomCSharp.Factory.GetRootFromString(csharpCode);
//            Assert.AreEqual("MyClass+MyNestedClass.myMethod", root.Classes.First().Classes.First().Methods.First().OuterName);
//        }

        [TestMethod, TestCategory(OuterNameCategory)]
        public void Can_get_outer_nestedType_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                            public class MyNestedClass
                            { 
                                public class MyNestedNestedClass {}
                            }
                        }";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("MyClass+MyNestedClass+MyNestedNestedClass", root.Classes.First().Classes.First().Classes.First().OuterName);
        }

        #endregion

        #region namespace tests
        [TestMethod, TestCategory(NamespaceNameCategory)]
        public void Can_get_class_namespace()
        {
            var csharpCode = @"
namespace NamespaceOuter
{namespace Namespace1
{
                        public class MyClass
                            { }
                        
}}";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("NamespaceOuter.Namespace1", root.ChildNamespaces.First().ChildNamespaces.First().Classes.First().Namespace);
        }

        [TestMethod, TestCategory(NamespaceNameCategory)]
        public void Can_get_class_empty_namespace()
        {
            var csharpCode = @"
namespace Namespace1
{
                        public class MyClass
                            { }
                        
}";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("Namespace1", root.Namespaces.First().Classes.First().Namespace);
        }

        [TestMethod, TestCategory(NamespaceNameCategory)]
        public void Can_get_empty_namespace()
        {
            var csharpCode = @"
namespace Namespace1
{
                        public class MyClass
                            { }
                        
}";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("", root.Namespaces.First().Namespace);
        }

        [TestMethod, TestCategory(NamespaceNameCategory)]
        public void Can_get_enums_namespace()
        {
            var csharpCode = @"
namespace NamespaceOuter
{ namespace Namespace1
{                       public enum MyEnum
                            { }
  } }                     ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("NamespaceOuter.Namespace1", root.Namespaces.First().Enums.First().Namespace);
        }


        [TestMethod, TestCategory(NamespaceNameCategory)]
        public void Can_get_struct_namespace()
        {
            var csharpCode = @"
namespace NamespaceOuter
{namespace Namespace1
{                        public struct MyStruct
                            { }
 }   }                    ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("NamespaceOuter.Namespace1", root.Namespaces.First().Structures.First().Namespace);
        }


        [TestMethod, TestCategory(NamespaceNameCategory)]
        public void Can_get_interface_namespace()
        {
            var csharpCode = @"
namespace NamespaceOuter
{namespace Namespace1
{                        public interface MyInterface
                            { }
}   }                     ";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("NamespaceOuter.Namespace1", root.Namespaces.First().Interfaces.First().Namespace);
        }

        [TestMethod]
        public void Get_nestedType_namespace()
        {
            var csharpCode = @"
namespace NamespaceOuter
{namespace Namespace1
{
                        public class MyClass
                        { 
                            public class MyNestedClass
                            { 
                                public class MyNestedNestedClass {}
                            }
                        }
}}";
            var root = RDom.CSharp.Load(csharpCode);
            Assert.AreEqual("NamespaceOuter.Namespace1",
                root.Namespaces.First().Classes.First().Classes.First().Classes.First()
                .Namespace);
        }
        #endregion

        #region referenced type tests
        [TestMethod, TestCategory(ReferencedTypeNamingCategory)]
        public void Can_get_names_for_referenced_type()
        {
            var csharpCode = @"
namespace MyNamespace
{
   public class Foo
   {
      public A<int> Bar1() {};
      public B Bar2() {};
      public C Bar3() {};
      public class B   { } 
   }
   public class A<T>   
   {
      public Foo.B Bar2() {};
      public FooX.B Bar3() {};
      public int Bar4() {};
      public T Bar4() {};
      public List<T> Bar4() {};
   }
}
public class C   { }
";
            var root = RDom.CSharp.Load(csharpCode);
            var methods = root.Namespaces.First().Classes.First().Methods.ToArray();
            var method = methods[0];
            var retTypeA = method.ReturnType;
            Assert.IsNotNull(retTypeA);
            Assert.AreEqual("A", retTypeA.Name, "Name A");
            Assert.AreEqual("MyNamespace.A", retTypeA.QualifiedName, "QualifiedName A");
            //Assert.AreEqual("A", retTypeA.OuterName, "OuterName A");
            Assert.AreEqual("MyNamespace", retTypeA.Namespace, "Namespace A");
            method = methods[1];
            var retTypeB = method.ReturnType;
            Assert.IsNotNull(retTypeB);
            Assert.AreEqual("B", retTypeB.Name, "Name B");
            Assert.AreEqual("MyNamespace.Foo.B", retTypeB.QualifiedName, "QualifiedName B");
            //Assert.AreEqual("Foo+B", retTypeB.OuterName, "OuterName B");
            Assert.AreEqual("MyNamespace", retTypeB.Namespace, "Namespace B");
            method = methods[2];
            var retTypeC = method.ReturnType;
            Assert.IsNotNull(retTypeC);
            Assert.AreEqual("C", retTypeC.Name, "Name C");
            Assert.AreEqual("C", retTypeC.QualifiedName, "QualifiedName C");
            //Assert.AreEqual("C", retTypeC.OuterName, "OuterName C");
            Assert.AreEqual("", retTypeC.Namespace, "Namespace C");
        }



        #endregion

    }
}
