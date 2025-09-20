import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/book.dart';

class ApiService {
  static const String baseUrl = 'http://192.168.1.5:5000/api'; // Replace with your API

  static Future<List<Book>> fetchBooks() async {
    final response = await http.get(Uri.parse('$baseUrl/Books/All'));

    if (response.statusCode == 200) {
      final List<dynamic> data = json.decode(response.body);
      return data.map((json) => Book.fromJson(json)).toList();
    } else {
      throw Exception('Failed to load books');
    }
  }
}