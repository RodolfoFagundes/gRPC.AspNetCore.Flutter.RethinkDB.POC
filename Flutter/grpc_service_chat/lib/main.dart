import 'package:flutter/material.dart';
import 'package:grpc_service_chat/services/chat/chat.pb.dart';

import 'services/chat/chat_service.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Chat GRPC',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: LoginPage(
        chatService: ChatService()..init(),
      ),
    );
  }
}

class LoginPage extends StatefulWidget {
  final ChatService chatService;

  const LoginPage({Key key, this.chatService}) : super(key: key);

  @override
  LoginState createState() => LoginState();
}

class LoginState extends State<LoginPage> {
  TextEditingController controller = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Please inform a username"),
      ),
      body: Center(
        child: Padding(
          padding: EdgeInsets.symmetric(horizontal: 20.0),
          child: Column(
            children: [
              TextField(
                controller: controller,
              ),
              MaterialButton(
                child: Text("Submit"),
                onPressed: () {
                  Navigator.of(context).push(
                    MaterialPageRoute(
                      builder: (context) =>
                          MessagePage(widget.chatService, controller.text),
                    ),
                  );
                },
              )
            ],
          ),
        ),
      ),
    );
  }
}

class MessagePage extends StatefulWidget {
  final ChatService service;
  final String username;
  MessagePage(this.service, this.username);

  @override
  _MessagePageState createState() => _MessagePageState();
}

class _MessagePageState extends State<MessagePage> {
  TextEditingController controller;

  Set<Message> messages;

  @override
  void initState() {
    super.initState();
    messages = Set();
    controller = TextEditingController();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Chat Page"),
      ),
      body: Center(
        child: Column(
          children: <Widget>[
            Padding(
              padding: EdgeInsets.symmetric(horizontal: 20.0),
              child: TextField(
                controller: controller,
              ),
            ),
            MaterialButton(
              child: Text("Submit"),
              onPressed: () {
                widget.service.join(widget.username, controller.text);
                controller.clear();
              },
            ),
            Flexible(
              child: StreamBuilder<Message>(
                  stream: widget.service.getMessage,
                  builder: (context, snapshot) {
                    if (!snapshot.hasData) {
                      return Center(
                        child: CircularProgressIndicator(),
                      );
                    }
                    messages.add(snapshot.data);

                    return ListView(
                      children: messages
                          .map((msg) => ListTile(
                                leading: Text(msg.user),
                                title: Text(msg.text),
                              ))
                          .toList(),
                    );
                  }),
            ),
          ],
        ),
      ),
    );
  }
}
