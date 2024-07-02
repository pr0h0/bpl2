using BPL2.Exceptions;
using BPL2.Expr;
using BPL2.Lexer;
using BPL2.Services;

namespace BPL2.Parser;

public class Parser
{
    private readonly List<Token> Tokens;
    private readonly List<Expression> Exprs;

    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
        Exprs = new();
    }

    public List<Expression> Parse()
    {
        while (!IsEOF())
        {
            var expr = ParseExpr();
            if (expr.Type != ExprType.EmptyExpr)
            {
                Exprs.Add(expr);
            }
        }
        return Exprs;
    }

    private bool IsEOF()
    {
        return Seek()?.Type == TokenType.EOF || Tokens.Count == 0;
    }

    private Expression ParseExpr()
    {
        var expr = ParseAssignment();
        Optional(TokenType.Semicolon);
        return expr;
    }

    private Expression ParseAssignment()
    {
        var expr = ParseTernary();

        if (Optional(TokenType.Assign) != null)
        {
            var value = ParseTernary();
            if (expr is IdentifierExpr || expr is ArrayAccessExpr || expr is ObjectAccessExpr)
            {
                return new AssignmentStatementExpr(expr, value);
            }

            throw new ParserException("Invalid assignment target", new Token(value.Type, value.Type));
        }

        return expr;
    }

    private Expression ParseTernary()
    {
        var expr = ParseLogical();

        if (Optional(TokenType.Question) != null)
        {
            var thenBranch = ParseExpr();
            Consume(TokenType.Colon, "Expected \":\" after ternary expression");
            var elseBranch = ParseExpr();

            expr = new IfStatementExpr(expr, thenBranch, elseBranch);
        }

        return expr;
    }

    private Expression ParseLogical()
    {
        var expr = ParseComparison();

        while (!IsEOF() && (Seek()?.Type == TokenType.And || Seek()?.Type == TokenType.Or))
        {
            var op = Consume(Seek()!.Type);
            var right = ParseComparison();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }

    private Expression ParseComparison()
    {
        var expr = ParseAddition();

        while (
            (!IsEOF() && Seek()?.Type == TokenType.Equal) ||
            Seek()?.Type == TokenType.NotEqual ||
            Seek()?.Type == TokenType.GreaterThan ||
            Seek()?.Type == TokenType.GreaterThanEqual ||
            Seek()?.Type == TokenType.LessThan ||
            Seek()?.Type == TokenType.LessThanEqual
        )
        {
            var op = Consume(Seek()!.Type);
            var right = ParseAddition();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }

    private Expression ParseAddition()
    {
        var expr = ParseMultiplication();

        while (
            !IsEOF() &&
            (Seek()?.Type == TokenType.Plus || Seek()?.Type == TokenType.Minus)
        )
        {
            var op = Consume(Seek()!.Type);
            var right = ParseMultiplication();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }

    private Expression ParseMultiplication()
    {
        var expr = ParseUnary();

        while (
            !IsEOF() &&
            (Seek()?.Type == TokenType.Multiply ||
                Seek()?.Type == TokenType.Exponent ||
                Seek()?.Type == TokenType.Divide ||
                Seek()?.Type == TokenType.Modulus)
        )
        {
            var op = Consume(Seek()!.Type);
            var right = ParseUnary();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }

    private Expression ParseUnary()
    {
        if (Seek()?.Type == TokenType.Not || Seek()?.Type == TokenType.Minus || Seek()?.Type == TokenType.Increment || Seek()?.Type == TokenType.Decrement)
        {
            var op = Seek()!;
            Optional(op.Type);
            var expr = ParsePrimary();
            return new UnaryExpr(op, expr);
        }
        else
        {
            var expr = ParsePrimary();
            if (Seek()?.Type == TokenType.Increment || Seek()?.Type == TokenType.Decrement)
            {
                var op = Consume(Seek()!.Type);
                return new UnaryExpr(op, expr);
            }

            return expr;
        }

    }

    private Expression ParsePrimary()
    {
        var expr = Seek();
        if (expr != null)
        {
            if (expr.Type == TokenType.NumberLiteral)
            {
                return new NumberLiteralExpr(Consume(TokenType.NumberLiteral).Value);
            }

            if (expr.Type == TokenType.StringLiteral)
            {
                return new StringLiteralExpr(Consume(TokenType.StringLiteral).Value);
            }

            if (expr.Type == TokenType.StringTemplate)
            {
                return ParseTemplateLiteral();
            }

            if (expr.Type == TokenType.OpenParen)
            {
                return ParseGroupingOrTupleLiteral();
            }

            if (expr.Type == TokenType.OpenCurly)
            {
                return ParseObjectLiteral();
            }

            if (expr.Type == TokenType.OpenBracket)
            {
                return ParseArrayLiteral();
            }

            if (expr.Type == TokenType.EOF)
            {
                Consume(TokenType.EOF);
                return new EmptyExpr();
            }

            if (expr.Type == TokenType.Identifier)
            {
                if (expr.Value == "if")
                {
                    return ParseIf();
                }
                if (expr.Value == "var" || expr.Value == "const")
                {
                    return ParseVariableDeclaration();
                }
                if (expr.Value == "for")
                {
                    return ParseFor();
                }
                if (expr.Value == "while" || expr.Value == "until")
                {
                    return ParseWhileUntil();
                }
                if (expr.Value == "do")
                {
                    return ParseDoWhileUntil();
                }
                if (expr.Value == "true" || expr.Value == "false")
                {
                    return new BooleanLiteralExpr(Consume(TokenType.Identifier).Value);
                }
                if (expr.Value == "null")
                {
                    Consume(TokenType.Identifier);
                    return new NullLiteralExpr();
                }

                if (expr.Value == "return")
                {
                    Consume(TokenType.Identifier);
                    return new ReturnStatementExpr(ParseExpr());
                }

                if (expr.Value == "continue")
                {
                    Consume(TokenType.Identifier);
                    return new ContinueStatementExpr();
                }

                if (expr.Value == "break")
                {
                    Consume(TokenType.Identifier);
                    return new BreakStatementExpr();
                }

                if (expr.Value == "throw")
                {
                    return ParseThrow();
                }

                if (expr.Value == "try")
                {
                    return ParseTryCatch();
                }

                if (expr.Value == "func")
                {
                    return ParseFunctionDeclaration();
                }

                if (expr.Value == "type")
                {
                    return ParseTypeDeclaration();
                }

                if (Seek(1)?.Type == TokenType.OpenParen) return ParseFunctionCall();
                if (Seek(1)?.Type == TokenType.Dot) return ParseObjectAccess();
                if (Seek(1)?.Type == TokenType.OpenBracket) return ParseArrayAccess();

                return new IdentifierExpr(Consume(TokenType.Identifier).Value);
            }
        }

        throw new ParserException($"Expected a primary expression but got {expr?.Type ?? "null"}", expr);
    }

    private Expression ParseArrayAccess()
    {
        var name = Consume(TokenType.Identifier);
        Consume(TokenType.OpenBracket);
        var index = ParseExpr();
        Consume(TokenType.CloseBracket);
        return new ArrayAccessExpr(name, index);
    }

    private Expression ParseArrayLiteral()
    {
        Consume(TokenType.OpenBracket);
        List<Expression> values = new();
        while (!IsEOF() && Seek()?.Type != TokenType.CloseBracket)
        {
            values.Add(ParseExpr());
            Optional(TokenType.Comma);
        }
        Consume(TokenType.CloseBracket, "Missing ] after array values");
        return new ArrayLiteralExpr(values);
    }

    private Expression ParseObjectAccess()
    {
        var name = Consume(TokenType.Identifier);
        Consume(TokenType.Dot);
        var field = Consume(TokenType.Identifier);
        return new ObjectAccessExpr(name, new StringLiteralExpr(field.Value));
    }

    private Expression ParseTypeDeclaration()
    {
        Consume(TokenType.Identifier);
        var name = Consume(TokenType.Identifier, "Missing type name");
        if (Seek()?.Type == TokenType.OpenCurly)
        {
            return ParseObjectTypeDeclaration(name);
        }
        if (Seek()?.Type == TokenType.OpenBracket)
        {
            return ParseArrayTypeDeclaration(name);
        }

        if (Seek()?.Type == TokenType.OpenParen)
        {
            return ParseTupleTypeDeclaration(name);
        }

        throw new ParserException("Invalid type declaration", Seek());
    }

    private Expression ParseTupleTypeDeclaration(Token name)
    {
        Consume(TokenType.OpenParen, "Missing ( after type name");
        List<Token> types = new();
        while (!IsEOF() && Seek()?.Type != TokenType.CloseParen)
        {
            var type = Consume(TokenType.Identifier, "Missing type");
            types.Add(type);
            Optional(TokenType.Comma);
        }
        Consume(TokenType.CloseParen, "Missing ) after type parameters");
        if (types.Count < 2) throw new ParserException("Tuple must have at least 2 types", Seek());
        return new TupleDeclarationExpr(name, types);
    }

    private Expression ParseArrayTypeDeclaration(Token name)
    {
        Consume(TokenType.OpenBracket, "Missing [ after type name");
        var type = Consume(TokenType.Identifier, "Missing type of array");
        Consume(TokenType.CloseBracket, "Missing ] after type of array");

        // TODO: Implement array declaration
        if (Math.Abs(1) == 1)
        {
            LogService.Error("=============== Array declaration needs to be reworked ===============");
            Thread.Sleep(3000);
            return new NumberLiteralExpr("1");
            //throw new NotImplementedException("Array declaration needs to be reworked");
        }

        return new TupleDeclarationExpr(name, new List<Token>() { type });
    }

    private Expression ParseObjectTypeDeclaration(Token name)
    {
        Consume(TokenType.OpenCurly, "Missing { after type name");
        List<Tuple<Token, Token>> fields = new();
        while (!IsEOF() && Seek()?.Type != TokenType.CloseCurly)
        {
            var typeName = Consume(TokenType.Identifier, "Missing field name");
            Consume(TokenType.Colon, "Missing : after field name");
            var type = Consume(TokenType.Identifier, "Missing type of field");
            fields.Add(Tuple.Create(typeName, type));
            Optional(TokenType.Comma);
        }
        Consume(TokenType.CloseCurly, "Missing } after type fields");
        return new TypeDeclarationExpr(name, fields);
    }

    private Expression ParseWhileUntil()
    {
        var type = Consume(TokenType.Identifier);
        Consume(TokenType.OpenParen, $"Expected '(' after {type.Value}");
        var condition = ParseExpr();
        Consume(TokenType.CloseParen, $"Expected ')' after {type.Value} condition");
        Expression? failsafe = Seek()?.Type == TokenType.OpenCurly ? null : ParseExpr();
        var body = ParseBlock();

        if (type.Value == "until")
        {
            return new UntilStatementExpr(condition, body, failsafe);
        }
        return new WhileStatementExpr(condition, body, failsafe);
    }

    private Expression ParseDoWhileUntil()
    {
        Consume(TokenType.Identifier);
        var failsafe = Seek()?.Type == TokenType.OpenCurly ? null : ParseExpr();

        var body = ParseBlock();
        var type = Consume(TokenType.Identifier, "Expected \"while\" or \"until\" after do's body");
        Consume(TokenType.OpenParen, $"Expected '(' after {type.Value}");
        var condition = ParseExpr();
        Consume(TokenType.CloseParen, $"Expected ')' after ${type.Value} condition");

        if (type.Value == "until")
        {
            return new DoUntilStatementExpr(condition, body, failsafe);
        }
        return new DoWhileStatementExpr(condition, body, failsafe);
    }

    private StringTemplateExpr ParseTemplateLiteral()
    {
        var expr = Consume(TokenType.StringTemplate);
        var value = expr.Value;

        List<Expression> exprs = new();

        if (value.Contains("${"))
        {
            var parts = value.Split("${").ToList();
            for (var i = 0; i < parts.Count; i++)
            {
                var part = parts[i];
                if (part == "") continue;
                if (part.Trim() == "")
                {
                    exprs.Add(new StringLiteralExpr(part));
                    continue;
                }
                if (i == 0)
                {
                    exprs.Add(new StringLiteralExpr(part));
                    continue;
                }
                var end = part.TrimStart().IndexOf('}');
                if (end == -1)
                {
                    exprs.Add(new StringLiteralExpr($"${{{part}"));
                    continue;
                }
                if (end == 0)
                {
                    throw new ParserException("Empty expression", expr);
                }

                var literalExpr = string.Join("", part.Take(end));
                List<Token> tokens = new Lexer.Lexer(literalExpr).Parse();

                exprs.AddRange(new Parser(tokens).Parse());

                exprs.Add(new StringLiteralExpr(part[(end + 1)..]));
            }
            var filteredExprs = exprs.Where((expr) => expr is not EmptyExpr).ToList();
            return new StringTemplateExpr(filteredExprs);
        }
        return new StringTemplateExpr(new List<Expression>() { new StringLiteralExpr(value) });
    }

    private FunctionDeclarationExpr ParseFunctionDeclaration()
    {
        Consume(TokenType.Identifier);
        var name =
            Optional(TokenType.Identifier) ??
            new Token(TokenType.Identifier, $"anonymous_${Seek()?.Line}_${DateTime.Now.Ticks}_${new Random().NextInt64()}", 0);
        Consume(TokenType.OpenParen);
        List<Tuple<Token, string>> parameters = new();
        while (!IsEOF() && Seek()?.Type != TokenType.CloseParen)
        {
            var functionName = Consume(TokenType.Identifier, "Missing parameter name");
            Consume(TokenType.Colon, "Missing : after parameter name");
            var type = Consume(TokenType.Identifier, "Missing type of parameter");
            parameters.Add(Tuple.Create(functionName, type.Value));
            Optional(TokenType.Comma);
        }

        Consume(TokenType.CloseParen, "Missing ) after function parameters");
        var typeOf = new Token(TokenType.Identifier, "VOID", 0);
        if (Seek()?.Type == TokenType.Colon)
        {
            Consume(TokenType.Colon, "Missing : after function parameters");
            typeOf = Consume(TokenType.Identifier, "Missing return type of function");
        }

        var body = ParseBlock();

        return new FunctionDeclarationExpr(name, parameters, body, typeOf);
    }

    private Expression ParseFunctionCall()
    {
        var name = Consume(TokenType.Identifier);
        Consume(TokenType.OpenParen, "Missing ( after function name");
        List<Expression> args = new();

        while (!IsEOF() && Seek()?.Type != TokenType.CloseParen)
        {
            args.Add(ParseExpr());
            Optional(TokenType.Comma);
        }

        Consume(TokenType.CloseParen, "Missing ) after function arguments");

        return new FunctionCallExpr(name, args);
    }

    private ForStatementExpr ParseFor()
    {
        Consume(TokenType.Identifier);
        Consume(TokenType.OpenParen, "Missing ( after for");
        var initializer = Seek()?.Type == TokenType.Semicolon ? new EmptyExpr() : ParseExpr();
        Optional(TokenType.Semicolon);
        var condition = Seek()?.Type == TokenType.Semicolon ? new EmptyExpr() : ParseExpr();
        Optional(TokenType.Semicolon);
        var increment = Seek()?.Type == TokenType.CloseParen ? new EmptyExpr() : ParseExpr();
        Consume(TokenType.CloseParen, "Missing ) after for");
        var failsafe = Seek()?.Type == TokenType.OpenCurly ? null : ParseExpr();
        var body = ParseBlock();

        return new ForStatementExpr(initializer, condition, increment, body, failsafe);
    }

    private VariableDeclarationStatementExpr ParseVariableDeclaration()
    {
        var type = Consume(TokenType.Identifier);
        var name = Consume(TokenType.Identifier, "Missing variable name");
        Consume(TokenType.Colon, "Missing : after variable name");
        var typeOf = Consume(TokenType.Identifier, "Missing type of variable");

        Consume(TokenType.Assign, "Missing = after variable declaration");
        var value = Seek()?.Type == TokenType.OpenCurly ? ParseObjectLiteral() : ParseExpr();

        return new VariableDeclarationStatementExpr(name, typeOf, value, type.Value == "var");
    }

    private Expression ParseObjectLiteral()
    {
        Consume(TokenType.OpenCurly);
        Dictionary<string, Expression> fields = new();
        while (!IsEOF() && Seek()?.Type != TokenType.CloseCurly)
        {
            var name = Consume(TokenType.Identifier, "Missing field name");
            Consume(TokenType.Colon, "Missing : after field name");
            var value = ParseExpr();
            fields.Add(name.Value, value);
            Optional(TokenType.Comma);
        }
        Consume(TokenType.CloseCurly, "Missing } after object fields");
        return new ObjectLiteralExpr(fields);
    }

    private IfStatementExpr ParseIf()
    {
        Consume(TokenType.Identifier);
        Consume(TokenType.OpenParen, "Missing ( after if");
        var condition = ParseExpr();
        Consume(TokenType.CloseParen, "Missing ) after if's condition");
        var thenBranch = ParseBlock();

        Expression? elseBranch = null;
        if (Seek()?.Type == TokenType.Identifier && Seek()?.Value == "else")
        {
            Consume(TokenType.Identifier);
            elseBranch =
                Seek()?.Type == TokenType.Identifier && Seek()?.Value == "if"
                    ? ParseIf()
                    : ParseBlock();
        }

        return new IfStatementExpr(condition, thenBranch, elseBranch);
    }

    private BlockStatementExpr ParseBlock()
    {
        Consume(TokenType.OpenCurly, "Missing { on block's start");
        List<Expression> stmts = new();

        while (!IsEOF() && Seek()?.Type != TokenType.CloseCurly)
        {
            stmts.Add(ParseExpr());
        }

        Consume(TokenType.CloseCurly, "Missing } on block's end");
        return new BlockStatementExpr(stmts);
    }

    private Expression ParseGroupingOrTupleLiteral()
    {
        Consume(TokenType.OpenParen);
        var expr = ParseExpr();
        if (Seek()?.Type == TokenType.Comma)
        {
            List<Expression> values = new() { expr };
            while (!IsEOF() && Seek()?.Type != TokenType.CloseParen)
            {
                Consume(TokenType.Comma);
                values.Add(ParseExpr());
            }
            Consume(TokenType.CloseParen, "Missing closing parenthesis )");
            return new TupleLiteralExpr(values);
        }
        Consume(TokenType.CloseParen, "Missing closing parenthesis )");
        return expr;
    }

    private Expression ParseThrow()
    {
        Consume(TokenType.Identifier);
        var expr = ParseTernary();
        return new ThrowStatementExpr(expr);
    }

    private Expression ParseTryCatch()
    {
        Consume(TokenType.Identifier);
        var tryBlock = ParseBlock();
        List<Tuple<Tuple<Token, Token>, Expression>> catchBlocks = new();
        while (Seek()?.Value == "catch")
        {
            Consume(TokenType.Identifier);
            Consume(TokenType.OpenParen);
            var exceptionType = Consume(TokenType.Identifier);
            var variable = Consume(TokenType.Identifier);

            var catchBlock = ParseBlock() as Expression;

            catchBlocks.Add(Tuple.Create(Tuple.Create(exceptionType, variable), catchBlock));
        }
        if (catchBlocks.Count == 0)
        {
            throw new ParserException("Expected \"catch\" after try block", Seek());
        }

        var finallyIdentifier = Seek();
        Expression? finallyBlock = null;
        if (finallyIdentifier?.Value == "finally")
        {
            finallyBlock = ParseBlock();
        }

        return new TryCatchStatementExpr(tryBlock, catchBlocks, finallyBlock);
    }

    private Token? Optional(string tokenType)
    {
        if (Seek()?.Type == tokenType)
        {
            return Consume(tokenType);
        }
        return null;
    }

    private Token? Seek(int index = 0)
    {
        if (Tokens.Count < index)
        {
            return null;
        }

        return Tokens[index];
    }

    private Token Consume(string token, string? message = null)
    {
        var temp = Seek() ?? throw new ParserException("Token not found");
        if (temp.Type != token)
        {
            throw new ParserException(message ?? $"Unexpected token, found <{temp.Type}>, but expected <{token}>", temp);
        }
        Tokens.RemoveAt(0);
        return temp;
    }
}