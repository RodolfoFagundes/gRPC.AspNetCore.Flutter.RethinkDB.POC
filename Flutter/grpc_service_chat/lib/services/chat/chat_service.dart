import 'dart:async';

import 'package:grpc/grpc.dart';
import 'package:grpc/grpc_connection_interface.dart';
import 'package:rxdart/rxdart.dart';

import 'chat.pbgrpc.dart';

class ChatService {
// class ChatService<C extends ClientChannelBase> {
  ClientChannelBase channel;
  ChatRoomClient client;

  final _psMessage = PublishSubject<Message>();

  Stream<Message> get getMessage => _psMessage.stream;

  void init() async {
    if (channel == null) {
      channel = ClientChannel(
        '192.168.1.198',
        port: 5001,
        options:
            const ChannelOptions(credentials: ChannelCredentials.insecure()),
      );
    }

    if (client == null && channel != null) {
      client = ChatRoomClient(
        channel,
        // options: CallOptions(
        //   timeout: const Duration(seconds: 30),
        // ),
      );
    }
  }

  Future<void> join(String user, String msg) async {
    Message message = Message(user: user, text: msg);

    Stream<Message> outgoingMessage() async* {
      await Future.delayed(Duration(milliseconds: 10));
      print('Sending message - user: ${message.user}, text: ${message.text}');
      _psMessage.sink..add(message);
      yield message;
    }

    final responses = client.join(outgoingMessage());

    await for (var response in responses) {
      print('Got message - user: ${response.user}, text: ${response.text}');
      _psMessage.sink..add(response);
    }
  }

  Future close() async {
    await _psMessage.close();
  }
}
