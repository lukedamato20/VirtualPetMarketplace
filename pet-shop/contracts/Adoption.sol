pragma solidity ^0.5.0;

contract Adoption {

    // unique type called an address. 
    //Addresses are Ethereum addresses, stored as 20 byte values.
    address[100] public adopters;

    // Adopting a pet
    // make sure petId is in range of our adopters array
        // if it is, make sure petId is in range of our adopters array
    function adopt(uint petId) public returns (uint) {
        require(petId >= 0 && petId <= 99);

        // msg.sender - The address of the person or smart contract who called this function
        adopters[petId] = msg.sender;

        return petId;
    }

    // Retrieving the adopters
    // memory - gives the data location for the variable.
    // view - the function will not modify the state of the contract.
    function getAdopters() public view returns (address[100] memory) {
        return adopters;
    }

}