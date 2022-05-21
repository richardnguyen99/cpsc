// CPSC 3400 - Homework 5
// Author: Minh-Hieu (Richard) Nguyen

/// <sumary>
///
/// As required in the assignment, this is the instructions as well as their
/// purposes to use the implementations.
///
/// `eval` is the main function that, as supposed, performs the main computation,
/// which takes a list of initialized variables and a string expression to
/// represent what task should be accomplished.
///
/// The idea of `eval` is based on stack-oriented programming like FORTH. When
/// the function encounters an alphabetical letter, it looks for the associated
/// variable and pushes to the predefined stack, if applicable. When it
/// encounters an operation that is defined in the assignment, a task is
/// performed according to that operation. Spaces and other characters will be
/// ignored. In this assignment, we assume that all variable names are single-
/// character and the value is integer.
///
/// The design of `eval` is to have an inner recursive function, `innerEval`,
/// to employ a stack. Each potential variable results a push to the stack.
/// Each operation will consume the stack, depending on the task.
///
/// `innerEval` uses `Seq.toList` to iterate the expression, which is a string
/// and a string is a sequence in F#. The result of the function is a char list.
///
/// If the expression is exhaustive (empty), `innerVal` returns the top of the
/// stack., which is consumed by `eval` as well.
///
/// If `innerEval` encounters a letter, which is checked by `System.Char.IsLetter()`,
/// it pushes the associated variable to the stack, and continues on the
/// expression if not exhaustive. The task is mainly performed by `getVar`.
/// See more details about `getVar` below.
///
/// If `innerEval` encouters '$', which is a swap operation, it swaps the
/// content of two top items on the stack and continues on the expression if not
/// exhaustive. The task mainly uses `popStack` to get the top item of stack.
///
/// If `innerEval` encounters '@', which is an assign operation, it assigns the
/// top item on the stack to the assigned variable. If the variable is not in
/// the list of variables yet, it pushes the new variable to the list. Otherwise,
/// it updates the according variable. Then, it returns the new list. An exception
/// will be thrown if the following task in the string expression is invalid.
///
/// If `innerEval` encounters an arithmetic operation, it pops two items from
/// the stack, computes the necessary operation and pushes the result to the
/// stack, then continues on the expression if not exhaustive. The task mainly
/// uses `popStack` to get the top two items of stack.
///
/// `getVar` is a helper function mainly for pushing a variable to the stack.
/// It takes a anme and a given list of variables as arugments and returns the
/// associated value of that in the given list, if applicable. An exception will
/// be thrown with `failwithf` if there is no such variable.
///
/// `popStack` is a helper function mainly in operations that consume items in
/// the stack. It pops the top item on the stack, and returns that item as well
/// as the rest of the stack. This allows multiple pop-stack operations. An
/// exception will be thrown with `failwithf` if the stack is empty (underflow).
///
/// `updateVarList` is a helper function mainly in the assignment operation. The
/// function recursively iterates through the given list of variables. If there
/// is no such variable, it creates a new one and add to the list. Otherwise,
/// it updates the value of that variable.
///
/// My assumptions for this program are that variables are named with a single
/// character, and that its value is integer.
///
/// </sumary>

type Variable = string * int
type VarList = Variable list

let rec getVar (name: char) (vars: VarList) : int =
    match vars with
    | [] -> failwithf "%c is undefined" name
    | (var, value)::rest when (System.Convert.ToString name) =  var -> value
    | (var, value)::rest -> getVar name rest

let rec updateVarList (newVar: Variable) (vars: VarList) : VarList =
    let newVarName, newVarValue = newVar
    match vars with
    | [] when newVarName = "" -> vars
    | [] -> newVar::vars
    | (var, value)::rest when var = newVarName -> (var,newVarValue)::(updateVarList ("", newVarValue) rest)
    | (var, value)::rest ->  (var, value)::(updateVarList newVar rest)

let popStack (stack: int list) : (int * int list) =
    match stack with
    | [] -> failwithf "Stack underflow"
    | top::rest -> (top, rest)

let stringify (chars: char list) : string = List.toArray chars |> System.String

let eval (vars: VarList)  (expr: string) : int=
    let arithmeticOps = Set.ofList ['+'; '-'; '*'; '/' ]

    let rec innerEval (vars: VarList) (stack: int list) (expr: string) : int =
        match (Seq.toList expr) with
        | [] -> stack.[0]

        | task::remainingExpr when (System.Char.IsLetter task) ->
            let stackItem = getVar task vars
            innerEval vars (stackItem::stack) (stringify remainingExpr)

        | task::remainingExpr when (task = '$') ->
            let (first, tempStack) = popStack stack
            let (second, newStack) = popStack tempStack
            innerEval vars (second::first::newStack) (stringify remainingExpr)


        | task::remainingExpr when (task = '@') ->
            match remainingExpr with
            | newVariable::remainingExpr when (System.Char.IsLetter newVariable) = false ->
                failwithf "Illegal variable name after @: %c" newVariable
            | newVariable::remainingExpr ->
                let (top, newStack) = popStack stack
                let newVarList = updateVarList (System.Convert.ToString newVariable, top) vars
                innerEval newVarList newStack (stringify remainingExpr)
            | _ -> failwith "Syntax error with @"

        | task::remainingExpr when (arithmeticOps.Contains task) ->
            let (first, tempStack) = popStack stack
            let (second, newStack) = popStack tempStack
            match task with
            | '+' -> innerEval vars ((second + first)::newStack) (stringify remainingExpr)
            | '-' -> innerEval vars ((second - first)::newStack) (stringify remainingExpr)
            | '*' -> innerEval vars ((second * first)::newStack) (stringify remainingExpr)
            | '/' -> innerEval vars ((second / first)::newStack) (stringify remainingExpr)
            | _ -> failwithf "%c operation is unknown" task

        | task::remainingExpr ->
            innerEval vars stack (stringify remainingExpr)

    innerEval vars [] expr
