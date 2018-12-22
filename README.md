# Game-Network-Protocol
(in development)

A network protocol to allow for a  secure, fast, cross-platform, communication between game and game server. The model is based on a peers-to-master approach.

A peers-to-master is a model where e.g. there's 100 peers all communicating with each other, however, a master server sits in between managing all of them. 
Since Peers can talk to each other, it is possible to issue a single command from a master-server directly to a single peer which then forwards it to all the other peers. 
This is way more efficient than traditional approaches. 

# Encryption
Uses Elliptical curve Diffie-Helman(ecdh) with RSA encryption. 
Once a secure key has been derived, the packets are then encrypted with AES.


# Packet exchange
Uses Lidgren.Network for reliable transport of packets. 
