import grpc
import os

import greet_pb2
import greet_pb2_grpc

def test_say_hello():
    server = "localhost:6000"  # hardcoded

    channel = grpc.insecure_channel(server)
    client = greet_pb2_grpc.GreeterStub(channel)

    response = client.SayHello(greet_pb2.HelloRequest(name="Manu"))
    assert "Manu" in response.message

