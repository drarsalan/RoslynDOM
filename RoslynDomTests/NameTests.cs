﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynDom;

namespace RoslynDomTests
{
    [TestClass]
    public class NameTests
    {
        #region simple name methods
        [TestMethod]
        public void Root_is_named_root()
        {
            var csharpCode = @"
                        using System.Diagnostics.Tracing;
                        namespace testing.Namespace1
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("Root", root.Name);
        }

        [TestMethod]
        public void Can_get_namespace_name()
        {
            var csharpCode = @"
                        namespace testing.Namespace1
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var ns = root.Namespaces.First();
            var symbol = ((IRoslynDom)ns).Symbol;
            Assert.AreEqual("Namespace1", ns.Name);
            Assert.AreEqual("Namespace1", symbol.MetadataName,"meta");
            Assert.AreEqual("Namespace1", symbol.Name);
        }

        [TestMethod]
        public void Can_get_class_name()
        {
            var csharpCode = @"
                        public class MyClass
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var cl = root.Classes.First();
            var symbol = ((IRoslynDom)cl).Symbol;
            Assert.AreEqual("MyClass", cl.Name);
            Assert.AreEqual("MyClass", symbol.MetadataName);
            Assert.AreEqual("MyClass", symbol.Name);
        }

        [TestMethod]
        public void Can_get_enums_name()
        {
            var csharpCode = @"
                        public enum MyEnum
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var en= root.Enums.First();
            var symbol = ((IRoslynDom)en).Symbol;
            Assert.AreEqual("MyEnum", en.Name);
            Assert.AreEqual("MyEnum", symbol.MetadataName);
            Assert.AreEqual("MyEnum", symbol.Name);
        }


        [TestMethod]
        public void Can_get_struct_name()
        {
            var csharpCode = @"
                        public struct MyStruct
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var st= root.Structures.First();
            var symbol = ((IRoslynDom)st).Symbol;
            Assert.AreEqual("MyStruct", st.Name);
            Assert.AreEqual("MyStruct", symbol.MetadataName);
            Assert.AreEqual("MyStruct", symbol.Name);
        }


        [TestMethod]
        public void Can_get_interface_name()
        {
            var csharpCode = @"
                        public interface MyInterface
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
           var inter =root.Interfaces.First();
            var symbol = ((IRoslynDom)inter).Symbol;
            Assert.AreEqual("MyInterface", inter.Name);
            Assert.AreEqual("MyInterface", symbol.MetadataName);
            Assert.AreEqual("MyInterface", symbol.Name);
        }

        [TestMethod]
        public void Can_get_field_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myField; }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var fld = root.Classes.First().Fields.First();
            var symbol = ((IRoslynDom)fld).Symbol;
            Assert.AreEqual("myField", fld.Name);
            Assert.AreEqual("myField", symbol.MetadataName);
            Assert.AreEqual("myField", symbol.Name);
        }

        [TestMethod]
        public void Can_get_multi_field_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myField, myField2; }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var fld =root.Classes.First().Fields.First();
            var fld2 = root.Classes.First().Fields.Last();
            var symbol = ((IRoslynDom)fld).Symbol;
             var symbol2 = ((IRoslynDom)fld2).Symbol;
           Assert.AreEqual("myField", fld.Name);
            Assert.AreEqual("myField2", fld2.Name);
            Assert.AreEqual("myField", symbol.MetadataName);
            Assert.AreEqual("myField2", symbol2.MetadataName);
            Assert.AreEqual("myField", symbol.Name);
            Assert.AreEqual("myField2", symbol2.Name);
        }

        [TestMethod]
        public void Can_get_property_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myProperty { get; } }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var pr = root.Classes.First().Properties.First();
            var symbol = ((IRoslynDom)pr).Symbol;
            Assert.AreEqual("myProperty", pr.Name);
            Assert.AreEqual("myProperty", symbol.MetadataName);
            Assert.AreEqual("myProperty", symbol.Name);
        }

        [TestMethod]
        public void Can_get_method_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myMethod(int x) { return x; } }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var me = root.Classes.First().Methods.First();
            var symbol = ((IRoslynDom)me).Symbol;
            Assert.AreEqual("myMethod", me.Name);
            Assert.AreEqual("myMethod", symbol.MetadataName);
            Assert.AreEqual("myMethod", symbol.Name);
        }

        [TestMethod]
        public void Can_get_nestedType_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public class MyNestedClass {  } }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var nestedType = root.Classes.First().Classes.First();
            var symbol = ((IRoslynDom)nestedType).Symbol;
            Assert.AreEqual("MyNestedClass", nestedType.Name);
            Assert.AreEqual("MyNestedClass", symbol.MetadataName);
            Assert.AreEqual("MyNestedClass", symbol.Name);
        }

     
        #endregion

        #region nested name tests
        [TestMethod]
        public void Can_get_nested_namespace_name()
        {
            var csharpCode = @"
                        namespace Namespace2
                        {
                        namespace testing.Namespace1
                        { }
                        }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var ns = root.Namespaces.First().Namespaces.First();
            var symbol = ((IRoslynDom)ns).Symbol;
            Assert.AreEqual("testing.Namespace1", symbol.MetadataName, "Meta");
            Assert.AreEqual("testing.Namespace1", ns.Name);
        }

        [TestMethod]
        public void Can_get_nested_class_name()
        {
            var csharpCode = @"
namespace Namespace1
{
                        public class MyClass
                            { }
                        
}";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyClass", root.Namespaces.First().Classes.First().Name);
        }

        [TestMethod]
        public void Can_get_nested_enums_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public enum MyEnum
                            { }
                        }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyEnum", root.Classes.First().Enums.First().Name);
        }

        [TestMethod]
        public void Can_get_nested_struct_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public struct MyStruct
                            { }
                        }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyStruct", root.Classes.First().Structures.First().Name);
        }


        [TestMethod]
        public void Can_get_nested_interface_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public interface MyInterface
                            { }
                        }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyInterface", root.Classes.First().Interfaces.First().Name);
        }

        [TestMethod]
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
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("myField", root.Classes.First().Classes.First().Fields.First().Name);
        }

        [TestMethod]
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
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("myProperty", root.Classes.First().Classes.First().Properties.First().Name);
        }

        [TestMethod]
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
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("myMethod", root.Classes.First().Classes.First().Methods.First().Name);
        }

        [TestMethod]
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
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyNestedNestedClass", root.Classes.First().Classes.First().Classes.First().Name);
        }


   
        #endregion

        #region keyword name tests
        [TestMethod]
        public void Can_get_keyword_namespace_name()
        {
            var csharpCode = @"
                        using System.Diagnostics.Tracing;
                        namespace @class
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("class", root.Namespaces.First().Name);
        }

        [TestMethod]
        public void Can_get_keyword_class_name()
        {
            var csharpCode = @"
                        public class @namespace
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("namespace", root.Classes.First().Name);
        }

        [TestMethod]
        public void Can_get_keyword_enums_name()
        {
            var csharpCode = @"
                        public enum @class
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("class", root.Enums.First().Name);
        }


        [TestMethod]
        public void Can_get_keyword_struct_name()
        {
            var csharpCode = @"
                        public struct @class
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("class", root.Structures.First().Name);
        }

        [TestMethod]
        public void Can_get_keyword_interface_name()
        {
            var csharpCode = @"
                        public interface @class
                            { }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("class", root.Interfaces.First().Name);
        }

        [TestMethod]
        public void Can_get_keyword_field_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int @class; }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("class", root.Classes.First().Fields.First().Name);
        }

        [TestMethod]
        public void Can_get_keyword_property_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int @class { get; } }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("class", root.Classes.First().Properties.First().Name);
        }

        [TestMethod]
        public void Can_get_keyword_method_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int @class(int x) { return x; } }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("class", root.Classes.First().Methods.First().Name);
        }

        [TestMethod]
        public void Can_get_keyword_nestedType_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { public class @public {  } }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("public", root.Classes.First().Classes.First().Name);
        }

        [TestMethod]
        public void Can_get_namespace_qualified_name()
        {
            var csharpCode = @"
                        namespace Namespace2
                        {
                        namespace testing.Namespace1
                            { }
                        }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("Namespace2.testing.Namespace1", root.Namespaces.First().Namespaces.First().QualifiedName);
        }
        #endregion

        #region qualified name tests
        [TestMethod]
        public void Can_get_class_qualified_name()
        {
            var csharpCode = @"
namespace Namespace1
{
                        public class MyClass
                            { }
                        
}";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("Namespace1.MyClass", root.Namespaces.First().Classes.First().QualifiedName);
        }

        [TestMethod]
        public void Can_get_qualified_enums_name()
        {
            var csharpCode = @"
 namespace Namespace1
{                       public enum MyEnum
                            { }
  }                      ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("Namespace1.MyEnum", root.Namespaces.First().Enums.First().QualifiedName);
        }


        [TestMethod]
        public void Can_get_qualified_struct_name()
        {
            var csharpCode = @"
namespace Namespace1
{                        public struct MyStruct
                            { }
 }                       ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("Namespace1.MyStruct", root.Namespaces.First().Structures.First().QualifiedName);
        }


        [TestMethod]
        public void Can_get_qualified_interface_name()
        {
            var csharpCode = @"
namespace Namespace1
{                        public interface MyInterface
                            { }
}                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("Namespace1.MyInterface", root.Namespaces.First().Interfaces.First().QualifiedName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Get_field_qualified_name_throws_exception()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myField; }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var x = root.Classes.First().Fields.First().QualifiedName;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Get_property_qualified_name_throws_exception()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myProperty { get; } }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var x = root.Classes.First().Properties.First().QualifiedName;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Can_get_method_qualified_name_throws_exception()
        {
            var csharpCode = @"
                        public class MyClass
                        { public int myMethod(int x) { return x; } }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            var x = root.Classes.First().Methods.First().QualifiedName;
        }

        [TestMethod]
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
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("Namespace1.MyClass.MyNestedClass.MyNestedNestedClass",
                root.Namespaces.First().Classes.First().Classes.First().Classes.First()
                .QualifiedName);
        }
        #endregion

        #region outer name tests
        [TestMethod]
        public void Can_get_outer_namespace_name()
        {
            var csharpCode = @"
                        namespace Namespace2
                        {
                        namespace testing.Namespace1
                            { }
                        }
                        ";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("Namespace2.testing.Namespace1", root.Namespaces.First().Namespaces.First().OuterName);
        }

        [TestMethod]
        public void Can_get_outer_class_name()
        {
            var csharpCode = @"
namespace Namespace1
{
                        public class MyClass
                            { }
                        
}";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyClass", root.Namespaces.First().Classes.First().OuterName);
        }

        [TestMethod]
        public void Can_get_outer_enums_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public enum MyEnum
                            { }
                        }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyClass.MyEnum", root.Classes.First().Enums.First().OuterName);
        }


        [TestMethod]
        public void Can_get_outer_struct_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public struct MyStruct
                            { }
                        }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyClass.MyStruct", root.Classes.First().Structures.First().OuterName);
        }


        [TestMethod]
        public void Can_get_outer_interface_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                        public interface MyInterface
                            { }
                        }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyClass.MyInterface", root.Classes.First().Interfaces.First().OuterName);
        }

        [TestMethod]
        public void Can_get_outer_field_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                            public class MyNestedClass
                            { 
                                public int myField; 
                            }
                        }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyClass.MyNestedClass.myField", root.Classes.First().Classes.First().Fields.First().OuterName);
        }

        [TestMethod]
        public void Can_get_outer_property_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                            public class MyNestedClass
                            { 
                                public int myProperty { get; } 
                            }
                        }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyClass.MyNestedClass.myProperty", root.Classes.First().Classes.First().Properties.First().OuterName);
        }

        [TestMethod]
        public void Can_get_outer_method_name()
        {
            var csharpCode = @"
                        public class MyClass
                        { 
                            public class MyNestedClass
                            { 
                                public int myMethod(int x) { return x; } 
                            }
                        }";
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyClass.MyNestedClass.myMethod", root.Classes.First().Classes.First().Methods.First().OuterName);
        }

        [TestMethod]
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
            var root = RDomFactory.GetRootFromString(csharpCode);
            Assert.AreEqual("MyClass.MyNestedClass.MyNestedNestedClass", root.Classes.First().Classes.First().Classes.First().OuterName);
        }
        
        #endregion

    }
}
