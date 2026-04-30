Finished the checkout method, from the pre-existing code added a new payment and change logic.
After user checksout they are prompted to buy again or exit.
Instead of view cart i made it so that cart is toggleable, because its a better design.



AI usage in this push:
 Before pushing I had AI check if i missed any errors and it pointed out that after checking out if the user chooses to go back, the car isnt cleared. The problem was fixed by setting cartCount to 0.