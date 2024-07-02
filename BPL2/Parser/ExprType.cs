namespace BPL2.Parser;

public static class ExprType
{
    public static string NumberLiteralExpr = "NUMBER_LITERAL_EXPR";
    public static string StringLiteralExpr = "STRING_LITERAL_EXPR";
    public static string StringTemplateExpr = "STRING_TEMPLATE_EXPR";
    public static string BooleanLiteralExpr = "BOOLEAN_LITERAL_EXPR";
    public static string NullLiteralExpr = "NULL_LITERAL_EXPR";
    public static string ArrayLiteralExpr = "ARRAY_LITERAL_EXPR";
    public static string ObjectLiteralExpr = "OBJECT_LITERAL_EXPR";
    public static string TupleLiteralExpr = "TUPLE_LITERAL_EXPR";
    public static string IdentifierExpr = "IDENTIFIER_EXPR";

    public static string UnaryExpr = "UNARY_EXPR";
    public static string BinaryExpr = "BINARY_EXPR";
    public static string TernaryExpr = "TERNARY_EXPR";
    public static string ObjectAccessExpr = "OBJECT_ACCESS_EXPR";
    public static string ArrayAccessExpr = "ARRAY_ACCESS_EXPR";

    public static string IfStatementExpr = "IF_STATEMENT_EXPR";
    public static string WhileStatementExpr = "WHILE_STATEMENT_EXPR";
    public static string DoWhileStatementExpr = "DO_WHILE_STATEMENT_EXPR";
    public static string UntilStatementExpr = "UNTIL_STATEMENT_EXPR";
    public static string DoUntilStatementExpr = "DO_UNTIL_STATEMENT_EXPR";
    public static string ForStatementExpr = "FOR_STATEMENT_EXPR";
    public static string BreakStatementExpr = "BREAK_STATEMENT_EXPR";
    public static string ContinueStatementExpr = "CONTINUE_STATEMENT_EXPR";
    public static string ReturnStatementExpr = "RETURN_STATEMENT_EXPR";
    public static string ThrowStatementExpr = "THROW_STATEMENT_EXPR";
    public static string TryCatchStatementExpr = "TRY_CATCH_STATEMENT_EXPR";

    public static string BlockStatementExpr = "BLOCK_STATEMENT_EXPR";
    public static string VariableDeclarationStatementExpr = "VARIABLE_DECLARATION_STATEMENT_EXPR";
    public static string AssignmentStatementExpr = "ASSIGNMENT_STATEMENT_EXPR";
    public static string EmptyExpr = "EMPTY_EXPR";
    public static string GroupingExpr = "GROUPING_EXPR"; // TODO: ??

    public static string FunctionDeclarationExpr = "FUNCTION_DECLARATION_EXPR";
    public static string FunctionCallExpr = "FUNCTION_CALL_EXPR";
    public static string TypeDeclarationExpr = "TYPE_DECLARATION_EXPR";
    public static string TupleDeclarationExpr = "TUPLE_DECLARATION_EXPR";

    public static string STLFunctionExpr = "STL_FUNCTION_DECLARATION";
}

