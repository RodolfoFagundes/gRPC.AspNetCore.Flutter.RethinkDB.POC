///
//  Generated code. Do not modify.
//  source: chat.proto
//
// @dart = 2.12
// ignore_for_file: annotate_overrides,camel_case_types,unnecessary_const,non_constant_identifier_names,library_prefixes,unused_import,unused_shown_name,return_of_invalid_type,unnecessary_this,prefer_final_fields

import 'dart:async' as $async;

import 'dart:core' as $core;

import 'package:grpc/service_api.dart' as $grpc;
import 'chat.pb.dart' as $0;
export 'chat.pb.dart';

class ChatRoomClient extends $grpc.Client {
  static final _$join = $grpc.ClientMethod<$0.Message, $0.Message>(
      '/chat.ChatRoom/join',
      ($0.Message value) => value.writeToBuffer(),
      ($core.List<$core.int> value) => $0.Message.fromBuffer(value));

  ChatRoomClient($grpc.ClientChannel channel,
      {$grpc.CallOptions? options,
      $core.Iterable<$grpc.ClientInterceptor>? interceptors})
      : super(channel, options: options, interceptors: interceptors);

  $grpc.ResponseStream<$0.Message> join($async.Stream<$0.Message> request,
      {$grpc.CallOptions? options}) {
    return $createStreamingCall(_$join, request, options: options);
  }
}

abstract class ChatRoomServiceBase extends $grpc.Service {
  $core.String get $name => 'chat.ChatRoom';

  ChatRoomServiceBase() {
    $addMethod($grpc.ServiceMethod<$0.Message, $0.Message>(
        'join',
        join,
        true,
        true,
        ($core.List<$core.int> value) => $0.Message.fromBuffer(value),
        ($0.Message value) => value.writeToBuffer()));
  }

  $async.Stream<$0.Message> join(
      $grpc.ServiceCall call, $async.Stream<$0.Message> request);
}
