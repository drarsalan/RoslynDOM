﻿using System.Linq;
using ApprovalTests;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynDom;
using RoslynDom.Common;
using RoslynDom.CSharp;

namespace RoslynDomTests
{
   [TestClass]
   public class MutabilityTests
   {
      private const string MutabilityCategory = "Mutability";

      #region Mutability tests
      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_add_copied_class_to_root()
      {
         string csharpCode = @"
            [Foo(""Fred"", bar:3, bar2:""George"")] 
            public class Bar{}           
            ";
         RDomRoot rDomRoot = RDom.CSharp.Load(csharpCode) as RDomRoot;
         IClass class1 = rDomRoot.RootClasses.First();
         IAttribute attribute = class1.Attributes.Attributes.First();
         IClass class2 = class1.Copy();
         rDomRoot.StemMembersAll.AddOrMove(class2);
         IClass[] classes = rDomRoot.Classes.ToArray();
         Assert.AreEqual(2, classes.Count());
         Assert.IsFalse(classes[0] == classes[1]); // reference equality fails
         Assert.IsTrue(classes[0].SameIntent(classes[1]));
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_add_copied_method_to_class()
      {
         string csharpCode = @"
            [Foo(""Fred"", bar:3, bar2:""George"")] 
            public class Bar
            {
                public string FooBar() {}
            }           
            ";
         IRoot root = RDom.CSharp.Load(csharpCode);
         RDomClass rDomClass = root.RootClasses.First() as RDomClass;
         IMethod method1 = rDomClass.Methods.First();
         IMethod method2 = method1.Copy();
         rDomClass.MembersAll.AddOrMove(method2);
         IMethod[] methods = rDomClass.Methods.ToArray();
         Assert.AreEqual(2, methods.Count());
         Assert.IsFalse(methods[0] == methods[1]); // reference equality fails
         Assert.IsTrue(methods[0].SameIntent(methods[1]));
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_change_attribute_name_and_output()
      {
         string csharpCode = @"
            [Foo(""Fred"", bar : 3, bar2 = 3.14, bar3=true)] 
            public class Bar
            {
                public string FooBar()
                {}
            }";
         IRoot root = RDom.CSharp.Load(csharpCode);
         IClass class1 = root.RootClasses.First();
         IAttribute attribute1 = class1.Attributes.Attributes.First();
         RDomAttribute attribute2 = class1.Attributes.Attributes.First().Copy() as RDomAttribute;
         Assert.IsTrue(attribute1.SameIntent(attribute2));
         attribute2.Name = "Foo2";
         Assert.IsFalse(attribute1.SameIntent(attribute2));
         Assert.AreEqual("Foo2", attribute2.Name);
         string expected = "            [Foo2(\"Fred\", bar : 3, bar2 = 3.14, bar3=true)] \r\n";
         string actual = RDom.CSharp.GetSyntaxNode(attribute2).ToFullString();
         SyntaxNode syntax1 = RDom.CSharp.GetSyntaxNode(class1);
         string actualClass = syntax1.ToFullString();
         Assert.AreEqual(expected, actual);
         Assert.AreEqual(csharpCode, actualClass);
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_change_class_name_and_output()
      {
         string csharpCode =
@"            [ Foo ( ""Fred"" , bar:3 , bar2=3.14 ) ] 
            public class Bar
            {
                [Bar(bar:42)] 
                public string FooBar() {}
            }";
         IRoot root = RDom.CSharp.Load(csharpCode);
         IClass class1 = root.RootClasses.First();
         RDomClass class2 = root.RootClasses.First().Copy() as RDomClass;
         Assert.IsTrue(class1.SameIntent(class2));
         class2.Name = "Bar2";
         string csharpCodeChanged = csharpCode.ReplaceFirst("class Bar", "class Bar2");
         Assert.IsFalse(class1.SameIntent(class2));
         Assert.AreEqual("Bar2", class2.Name);
         string origCode = RDom.CSharp.GetSyntaxNode(class1).ToFullString();
         Assert.AreEqual(csharpCode, origCode);
         string newCode = RDom.CSharp.GetSyntaxNode(class2).ToFullString();
         Assert.AreEqual(csharpCodeChanged, newCode);
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_change_generic_class_name_and_output()
      {
         string csharpCode = @"
            [Foo(""Fred"", bar:3, bar2=3.14)] 
            public class Bar<T>
            {
                private int fooish;
                [Bar( bar:42)] 
                public string FooBar() {}
            }           
            ";
         IRoot root = RDom.CSharp.Load(csharpCode);
         IClass class1 = root.RootClasses.First();
         RDomClass class2 = root.RootClasses.First().Copy() as RDomClass;
         string newCode = RDom.CSharp.GetSyntaxNode(class2).ToFullString();
         Assert.IsTrue(class1.SameIntent(class2));
         class2.Name = "Bar2";
         Assert.IsFalse(class1.SameIntent(class2));
         Assert.AreEqual("Bar2", class2.Name);
         newCode = RDom.CSharp.GetSyntaxNode(class2).ToFullString();
         string expected = "            [Foo(\"Fred\", bar:3, bar2=3.14)] \r\n            public class Bar2<T>\r\n            {\r\n                private int fooish;\r\n                [Bar( bar:42)] \r\n                public string FooBar() {}\r\n            }           \r\n";
         Assert.AreEqual(expected, newCode);
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_remove_params_from_method()
      {
         string csharpCode = @"
            public class Bar
            {
                public string FooBar(int bar1, string bar2) {}
            }           
            ";
         IRoot root = RDom.CSharp.Load(csharpCode);
         RDomMethod method = root.RootClasses.First().Methods.First() as RDomMethod;
         IParameter param = method.Parameters.First();
         Assert.AreEqual(2, method.Parameters.Count());
         method.Parameters.Remove(param);
         Assert.AreEqual(1, method.Parameters.Count());
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_remove_type_params_from_class()
      {
         string csharpCode = @"
            public class Bar<T1, T2, T3>
            {
                public string FooBar(int bar1, string bar2) {}
            }           
            ";
         IRoot root = RDom.CSharp.Load(csharpCode);
         RDomClass class1 = root.RootClasses.First() as RDomClass;
         ITypeParameter param = class1.TypeParameters.Skip(1).First();
         Assert.AreEqual(3, class1.TypeParameters.Count());
         class1.TypeParameters.Remove(param);
         Assert.AreEqual(2, class1.TypeParameters.Count());
         Assert.AreEqual("T1", class1.TypeParameters.First().Name);
         Assert.AreEqual("T3", class1.TypeParameters.Last().Name);
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_clear_type_params_from_class()
      {
         string csharpCode = @"
            public class Bar<T1, T2, T3>
            {
                public string FooBar(int bar1, string bar2) {}
            }           
            ";
         IRoot root = RDom.CSharp.Load(csharpCode);
         RDomClass class1 = root.RootClasses.First() as RDomClass;
         Assert.AreEqual(3, class1.TypeParameters.Count());
         class1.TypeParameters.Clear();
         Assert.AreEqual(0, class1.TypeParameters.Count());
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_remove_member_from_class()
      {
         string csharpCode = @"
            public class Bar<T1, T2, T3>
            {
                private int foo;
                public string FooBar(int bar1, string bar2) {}
            }           
            ";
         IRoot root = RDom.CSharp.Load(csharpCode);
         RDomClass class1 = root.RootClasses.First() as RDomClass;
         Assert.AreEqual(2, class1.Members.Count());
         class1.MembersAll.Clear();
         Assert.AreEqual(0, class1.Members.Count());
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_remove_stem_member_from_root()
      {
         string csharpCode = @"
            using System;
            public class Bar{}           
            public struct Bar2{}             ";
         RDomRoot root = RDom.CSharp.Load(csharpCode) as RDomRoot;
         Assert.AreEqual(3, root.StemMembers.Count());
         IClass class1 = root.Classes.First();
         root.StemMembersAll.Remove(class1);
         Assert.AreEqual(2, root.StemMembers.Count());
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_clear_stem_members_from_root()
      {
         string csharpCode = @"
            using System;
            public class Bar{}           
            public struct Bar2{}             ";
         RDomRoot root = RDom.CSharp.Load(csharpCode) as RDomRoot;
         Assert.AreEqual(3, root.StemMembers.Count());
         IClass class1 = root.Classes.First();
         root.ClearStemMembers();
         Assert.AreEqual(0, root.StemMembers.Count());
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_copy_multi_member_root()
      {
         string csharpCode = @"
            using System;
            public class Bar{}           
            public struct Bar2{}           
            public enum Bar3{}           
            public interface Bar4{}           
            ";
         RDomRoot rDomRoot = RDom.CSharp.Load(csharpCode) as RDomRoot;
         IRoot rDomRoot2 = rDomRoot.Copy();
         RDomClass class1 = rDomRoot.RootClasses.First() as RDomClass;
         Assert.IsTrue(rDomRoot.SameIntent(rDomRoot2));
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_build_syntax_for_multi_member_root()
      {
         string csharpCode = @"
            using System;
            using System.Data;
            public class Bar{}           
            public struct Bar2{}           
            public enum Bar3{}           
            public interface Bar4{}";
         RDomRoot rDomRoot = RDom.CSharp.Load(csharpCode) as RDomRoot;
         SyntaxNode output = RDom.CSharp.GetSyntaxNode(rDomRoot);
         Assert.AreEqual(csharpCode, output.ToFullString());
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_change_variable_to_non_implicit()
      {
         string csharpCode = @"
public class Bar
{
   private string lastName;
   public void Foo()
   {
      var ret = lastName;
   }
}";
         RDomRoot root = RDom.CSharp.Load(csharpCode) as RDomRoot;
         string output = RDom.CSharp.GetSyntaxNode(root).ToFullString();
         Assert.AreEqual(csharpCode, output, "Inital");

         IDeclarationStatement statement = root.RootClasses.First().Methods.First().Statements.First() as IDeclarationStatement ;
         statement.IsImplicitlyTyped = false;
         output = RDom.CSharp.GetSyntaxNode(root).ToFullString();
         string newCode = csharpCode.Replace("var", "System.String");
         Assert.AreEqual(newCode, output, "After change");
      }

      [TestMethod, TestCategory(MutabilityCategory)]
      public void Can_change_variable_to_non_aliased()
      {
         string csharpCode = @"
public class Bar
{
   private string lastName;
   public void Foo()
   {
      var ret = lastName;
   }
}";
         RDomRoot root = RDom.CSharp.Load(csharpCode) as RDomRoot;
         string output = RDom.CSharp.GetSyntaxNode(root).ToFullString();
         Assert.AreEqual(csharpCode, output, "Inital");

         IDeclarationStatement statement = root.RootClasses.First().Methods.First().Statements.First() as IDeclarationStatement;
         statement.IsImplicitlyTyped = false;
         statement.Type.DisplayAlias  = true;
         output = RDom.CSharp.GetSyntaxNode(root).ToFullString();
         string newCode = csharpCode.Replace("var", "string");
         Assert.AreEqual(newCode, output, "After change");
      }
      #endregion
   }
}
