﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDom.Common;

namespace RoslynDom.CSharp
{
    public class RDomAssignmentStatementFactory
         : RDomStatementFactory<RDomAssignmentStatement, ExpressionStatementSyntax>
    {
        protected  override IStatementCommentWhite CreateItemFrom(SyntaxNode syntaxNode, IDom parent, SemanticModel model)
        {
            var syntax = syntaxNode as ExpressionStatementSyntax;
            var newItem = new RDomAssignmentStatement(syntaxNode, parent,model);

            var binary = syntax.Expression as BinaryExpressionSyntax;
            if (binary == null) throw new InvalidOperationException();
            // TODO: handle all the other kinds of assigments here (like +=)
            if (binary.CSharpKind() != SyntaxKind.SimpleAssignmentExpression) { throw new NotImplementedException(); }
            var left = binary.Left as ExpressionSyntax;
            // Previously tested for identifier here, but can also be SimpleMemberAccess and ElementAccess expressions
            // not currently seeing value in testing for the type. Fix #46
            // Also changed Name to Left and string to expression
            var right = binary.Right;
            var expression = right as ExpressionSyntax;
            if (expression == null) throw new InvalidOperationException();
            newItem.Left = RDomFactoryHelper.GetHelperForExpression().MakeItems(left, newItem, model).FirstOrDefault();
            newItem.Expression = RDomFactoryHelper.GetHelperForExpression().MakeItems(expression, newItem, model).FirstOrDefault();

            return newItem ;
        }
        public override IEnumerable<SyntaxNode> BuildSyntax(IStatementCommentWhite item)
        {
            var itemAsT = item as IAssignmentStatement;
            var leftSyntax = RDomCSharpFactory.Factory.BuildSyntax(itemAsT.Left);
            var expressionSyntax = RDomCSharpFactory.Factory.BuildSyntax(itemAsT.Expression );

            var assignmentSyntax = SyntaxFactory.BinaryExpression(SyntaxKind.SimpleAssignmentExpression, 
                            (ExpressionSyntax)leftSyntax, (ExpressionSyntax)expressionSyntax);
            var node = SyntaxFactory.ExpressionStatement(assignmentSyntax );

            return item.PrepareForBuildSyntaxOutput(node);

        }
        public override bool CanCreateFrom(SyntaxNode syntaxNode)
        {
            var statement = syntaxNode as ExpressionStatementSyntax;
            if (statement == null) { return false; }
            return statement.Expression.CSharpKind() == SyntaxKind.SimpleAssignmentExpression;
        }
    }
}