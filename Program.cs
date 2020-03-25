using System;
using System.Collections.Generic;
using System.Text;

namespace MulitList_Starter
{
    class Program
    {
        // This is my comment noting that: upon recieveing the starter file there was a class object "tester" 
        //and the method "RunTests"  that causes the prgrm not to compile 
        //missing specificie RunTests in order for the starter file to compile
        // in a professional setting I would comment out this code with note asking for explaineation and then send out an email or 
        //slack message that asks for an explanation
        //
        static void Main(string[] args)
        {
            (new UserInterface()).RunProgram();

            // Or, you could go with the more traditional:
            // UserInterface ui = new UserInterface();
            // ui.RunProgram();
        }
    }

    // Bit of a hack, but still an interesting idea....
    enum MenuOptions
    {
        // DO NOT USE ZERO!
        // (TryParse will set choice to zero if a non-number string is typed,
        // and we don't want to accidentally set nChoice to be a member of this enum!)
        QUIT = 1,
        ADD_BOOK,
        PRINT_BY_AUTHOR,
        PRINT_BY_TITLE,
        REMOVE_BOOK,
        RUN_TESTS
    }

    class UserInterface
    {
        MultiLinkedListOfBooks theList;
        public void RunProgram()
        {
            int nChoice;
            theList = new MultiLinkedListOfBooks();

            do // main loop
            {
                Console.WriteLine("Your options:");
                Console.WriteLine("{0} : End the program", (int)MenuOptions.QUIT);
                Console.WriteLine("{0} : Add a book", (int)MenuOptions.ADD_BOOK);
                Console.WriteLine("{0} : Print all books by author", (int)MenuOptions.PRINT_BY_AUTHOR);
                Console.WriteLine("{0} : Remove a Book", (int)MenuOptions.REMOVE_BOOK);
                Console.WriteLine("{0} : RUN TESTS", (int)MenuOptions.RUN_TESTS);
                if (!Int32.TryParse(Console.ReadLine(), out nChoice))
                {
                    Console.WriteLine("You need to type in a valid, whole number!");
                    continue;
                }
                switch ((MenuOptions)nChoice)
                {
                    case MenuOptions.QUIT:
                        Console.WriteLine("Thank you for using the multi-list program!");
                        break;
                    case MenuOptions.ADD_BOOK:
                        this.AddBook();
                        break;
                    case MenuOptions.PRINT_BY_AUTHOR:
                        theList.PrintByAuthor();
                        break;
                    case MenuOptions.REMOVE_BOOK:
                        this.RemoveBook();
                        break;
                    //case MenuOptions.RUN_TESTS:
                    //    AllTests tester = new AllTests();
                    //    tester.RunTests();
                    //    break;
                    default:
                        Console.WriteLine("I'm sorry, but that wasn't a valid menu option");
                        break;

                }
            } while (nChoice != (int)MenuOptions.QUIT);
        }

        public void AddBook()
        {
            Console.WriteLine("ADD A BOOK!");

            Console.WriteLine("Author name?");
            string author = Console.ReadLine();

            Console.WriteLine("Title?");
            string title = Console.ReadLine();

            double price = -1;
            while (price < 0)
            {
                Console.WriteLine("Price?");
                if (!Double.TryParse(Console.ReadLine(), out price))
                {
                    Console.WriteLine("I'm sorry, but that's not a number!");
                    price = -1;
                }
                else if (price < 0)
                {
                    Console.WriteLine("I'm sorry, but the number must be zero, or greater!!");
                }
            }

            ErrorCode ec = theList.Add(author, title, price);

            if (ec == ErrorCode.OK)
                Console.WriteLine("successfully added a book");
            else
                Console.WriteLine("failed to add a book");//make thses more fancy
        }

        public void RemoveBook()
        {
            Console.WriteLine("REMOVE A BOOK!");

            Console.WriteLine("Author name?");
            string author = Console.ReadLine();

            Console.WriteLine("Title?");
            string title = Console.ReadLine();

            ErrorCode ec = theList.Remove(author, title);

            if (ec == ErrorCode.OK)
                Console.WriteLine("removed a book!");
            else
                Console.WriteLine("failed to remove a book");
        }
    }

    enum ErrorCode
    {
        OK,
        DuplicateBook,
        BookNotFound
    }

    class MultiLinkedListOfBooks
    {
        private Book firstAuthor;
        private class Book
        {
            public string auth;
            public string title;
            public double price;
            public Book next;

            public Book(double p)
            {
                price = p;
            }
            public Book(string a, string t)
            {
                auth = a;
                title = t;
            }
            public Book(string a, string t, double p)
            {
                auth = a;
                title = t;
                price = p;
            }
            public Book(string a, string t, double p, Book givenext)
            {
                next = givenext;
            }

            // This compares 'this' book to the other book's author and title.
            // This version FIRST compares by author, and if they're the same THEN compares by title
            public int CompareAuthorTHENTitle(string otherBooksAuthor, string otherBooksTitle)
            {
                int comapredNum3 = string.Compare(auth, otherBooksAuthor/*, StringComparison.OrdinalIgnoreCase*/);
                int comparedNum4 = string.Compare(title, otherBooksTitle/*, StringComparison.OrdinalIgnoreCase*/);

                //rules of string compare
                //returns a num less than zero if the first string is greater than the second alphabetically
                // returns zero if they are equal
                //returns a num greater than zero is returned then the second value is what if first alphabitacally

                if (comapredNum3 == 0)
                {
                    comapredNum3 = comparedNum4;
                }
                return comapredNum3;
            }

            // This compares 'this' book to the other book's author and title.
            // This version FIRST compares by title, and if they're the same THEN compares by author
            public int CompareTitleTHENAuthor(string otherBooksAuthor, string otherBooksTitle)
            {   // You may (or may not) need this method
                int comapredNum = string.Compare(title, otherBooksTitle/*,StringComparison.OrdinalIgnoreCase*/);
                int comparedNum2 = string.Compare(auth, otherBooksAuthor/*, StringComparison.OrdinalIgnoreCase*/);

                if (comapredNum == 0)
                {
                    comapredNum = comparedNum2;
                    return comapredNum;
                }
                else
                    return comapredNum;
            }

            // Print out the book info (author, title, price).
            public void Print()
            {
                Console.WriteLine("Book Author:{0}", auth);
                Console.WriteLine("Book Title:{0}", title);
                Console.WriteLine("Price:${0}", price);
            }
        }

        public ErrorCode Add(string author, string title, double price)
        {
            // If the book is already in the list (author, title), then
            // do the following:
            //return ErrorCode.DuplicateBook;

            // having multiple books with the same author, but different titles, or 
            // multiple books with the same title, but different authors, is fine.

            // two books with the same author & title should be identified as duplicates,
            // even if the prices are different.
            Book newBook = new Book(author, title, price);

            if (firstAuthor == null || firstAuthor.CompareAuthorTHENTitle(author, title) > 0)// if the list is not yet initalized or the first author should remain  
                                                                                             //the first item comapred to what we are trying to add
            {
                newBook.next = firstAuthor;
                firstAuthor = newBook;// then add to front, wait right?
                return ErrorCode.OK;
            }
            else if (firstAuthor.CompareAuthorTHENTitle(author, title) == 0)// if the book we are trying to place is already in the list bothe auth and title then dont add it 
            {
                return ErrorCode.DuplicateBook;
            }
            else
            {
                // locate the node before the point of instertion
                Book curr = firstAuthor;
                while (curr.next != null && curr.next.CompareAuthorTHENTitle(author, title) < 0)// walking through the list up until the value just before POI
                {
                    curr = curr.next;
                }
                if (curr.next == null)
                {
                    curr.next = newBook;
                    return ErrorCode.OK;
                }
                    
                if (curr.next.CompareAuthorTHENTitle(newBook.auth, newBook.title) == 0)
                    return ErrorCode.DuplicateBook;

                if (curr.next.CompareAuthorTHENTitle(author, title) > 0 )
                {
                    newBook.next = curr.next;
                    curr.next = newBook;
                    return ErrorCode.OK;
                }                // greater than zero should work because we already checked for == and 
                //if should be add to the list in order
                // now that we have arrived at the place just before where we need to insert
                
                curr.next = newBook;// add to end
                return ErrorCode.OK;
            }
        }

        public void PrintByAuthor()
        {
            // if there are no books, then print out a message saying that the list is empty
            if (firstAuthor == null)
            {
                Console.WriteLine("The List is Currently Empty");
            }
            else
            {
                Book tempstart = firstAuthor;
                Console.WriteLine();
                while (tempstart != null)
                {
                    tempstart.Print();
                    Console.WriteLine();
                    tempstart = tempstart.next;
                }
            }
        }

        public ErrorCode Remove(string author, string title)
        {
            // if there isn't an exact match, then do the following:
            //return ErrorCode.BookNotFound;
            // (this includes finding a book by the given author, but with a different title,
            // or a book with the given title, but a different author)
            Book current = firstAuthor;
            Book givenBook = new Book(author, title);

            if (firstAuthor == null || firstAuthor.CompareAuthorTHENTitle(author, givenBook.title) == 0)// two ways of getting the same data
                                                                                                        // if there is no list or the first item in the list is whhat we are looking for
            {
                if (firstAuthor != null)
                    firstAuthor = firstAuthor.next;
                Console.WriteLine("Book Removed");
                return ErrorCode.OK;
            }
            while (current.next != null && current.next.CompareAuthorTHENTitle(givenBook.auth, givenBook.title) != 0)//while in list go through the list until we find the val
                current = current.next;
            if (current.next != null)// remove that val
            {
                current.next = current.next.next;
                Console.WriteLine("Book Removed!");// simple return if past alphbitcally past target or if null
            }

            return ErrorCode.BookNotFound;
        }
    }
}
