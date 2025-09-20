class Book {
  final int id;
  final String title;
  final String description;
  final String author;
  final String imageUrl;

  Book({
    required this.id,
    required this.title,
    required this.description,
    required this.author,
    required this.imageUrl,
  });

  factory Book.fromJson(Map<String, dynamic> json) {
    String baseUrl = 'http://192.168.1.5:5000/api/Books/GetImage/';
    return Book(
      id: json['id'],
      title: json['title'],
      description: json['description'],
      author: "${json['author']['firstName']} ${json['author']['lastName']}",
      imageUrl: baseUrl + json['imageName'], // 👈 build the full image URL
    );
  }
}
